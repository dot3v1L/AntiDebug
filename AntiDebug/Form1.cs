using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace AntiDebug
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var open = new OpenFileDialog())
            {
                open.Filter = ".exe|*.exe";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = open.FileName;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var save = new SaveFileDialog())
            {
                save.Filter = ".exe|*.exe";
                save.FileName = String.Format("{0}_anti", Path.GetFileNameWithoutExtension(textBox1.Text));
                if (save.ShowDialog() == DialogResult.OK)
                {
                    AntiDebag(textBox1.Text,save.FileName);
                    MessageBox.Show("Oopss");
                }
            }
        }

        private void AntiDebag(string path, string outPath)
        {
            AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(path);
            TypeDefinition t = assembly.MainModule.GetType("<Module>");
            MethodDefinition cctor = null; // <Module>.cctor
            foreach (MethodDefinition m in t.Methods)
            {
                if (m.Name == ".cctor" && m.IsConstructor)
                {
                    cctor = m;
                    break;
                }
            }
            if (cctor == null) // если ни один cctor не найден, создать новый
            {
                cctor = new MethodDefinition(
                    ".cctor",
                    Mono.Cecil.MethodAttributes.Private |
                    Mono.Cecil.MethodAttributes.Static |
                    Mono.Cecil.MethodAttributes.HideBySig |
                    Mono.Cecil.MethodAttributes.SpecialName |
                    Mono.Cecil.MethodAttributes.RTSpecialName
                    , assembly.MainModule.Import(typeof(void)));
                t.Methods.Add(cctor);
            }
            if (cctor.Body.Instructions.Count > 0 && cctor.Body.Instructions.Last<Instruction>().OpCode == OpCodes.Ret)
            {
                cctor.Body.Instructions.Remove(cctor.Body.Instructions.Last<Instruction>()); // если cctor уже существовал удалить последний
            }
            MethodDefinition AntiDebug = new MethodDefinition(// Метод antidebug (управляемый), который будет выполнен в новом потоке
                "AntiDebug",
                Mono.Cecil.MethodAttributes.Static |
                Mono.Cecil.MethodAttributes.Private,
                assembly.MainModule.Import(typeof(void)));
            
            ILProcessor CtorProcessor = cctor.Body.GetILProcessor();
            CtorProcessor.Append(CtorProcessor.Create(OpCodes.Ldnull));
            CtorProcessor.Append(CtorProcessor.Create(OpCodes.Ldftn, AntiDebug));
            CtorProcessor.Append(CtorProcessor.Create(OpCodes.Newobj,
                assembly.MainModule.Import(typeof(ThreadStart).GetConstructor(new Type[] { typeof(object), typeof(IntPtr) })))); // если указано несколько методов с тем же именем Тип  параметр 
            CtorProcessor.Append(CtorProcessor.Create(OpCodes.Newobj, assembly.MainModule.Import(typeof(Thread).GetConstructor(new Type[] { typeof(ThreadStart) }))));
            CtorProcessor.Append(CtorProcessor.Create(OpCodes.Call, assembly.MainModule.Import(typeof(Thread).GetMethod("Start", Type.EmptyTypes))));
            CtorProcessor.Append(CtorProcessor.Create(OpCodes.Ret));

            ILProcessor AntiProcessor = AntiDebug.Body.GetILProcessor();
            Instruction First = AntiProcessor.Create(OpCodes.Call, assembly.MainModule.Import(typeof(Debugger).GetMethod("get_IsAttached")));
            Instruction Exit = AntiProcessor.Create(OpCodes.Ldc_I4_0);
            AntiProcessor.Append(First);
            AntiProcessor.Append(AntiProcessor.Create(OpCodes.Brtrue_S, Exit));
            AntiProcessor.Append(AntiProcessor.Create(OpCodes.Call, assembly.MainModule.Import(typeof(Debugger).GetMethod("IsLogging"))));
            AntiProcessor.Append(AntiProcessor.Create(OpCodes.Brtrue_S, Exit));
            AntiProcessor.Append(AntiProcessor.Create(OpCodes.Ldc_I4, 1000));
            AntiProcessor.Append(AntiProcessor.Create(OpCodes.Call, assembly.MainModule.Import(typeof(Thread).GetMethod("Sleep", new Type[] { typeof(Int32) }))));
            AntiProcessor.Append(AntiProcessor.Create(OpCodes.Br_S, First));
            AntiProcessor.Append(Exit);
            AntiProcessor.Append(AntiProcessor.Create(OpCodes.Call, assembly.MainModule.Import(typeof(Environment).GetMethod("Exit", new Type[] { typeof(int) }))));
            AntiProcessor.Append(AntiProcessor.Create(OpCodes.Ret));
            t.Methods.Add(AntiDebug);
            assembly.Write(outPath);
        }
    }
}
