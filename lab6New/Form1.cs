using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab6New
{
    public partial class Form1 : Form
    {
        private readonly SoldierComposite rootComposite;

        public Form1()
        {
            InitializeComponent();
            //створення кореневого компонента строю
            rootComposite = new SoldierComposite("Строй");




            //додавання команди, яку може виконати лише один солдат
            SoldierLeaf singleSoldier = new SoldierLeaf("Одинокий солдат");
            rootComposite.Add(singleSoldier);

            //додавання команди, яку може виконати тільки група солдат
            SoldierComposite group = new SoldierComposite("Група солдат");
            group.Add(new SoldierLeaf("Солдат 1"));
            group.Add(new SoldierLeaf("Солдат 2"));
            group.Add(new SoldierLeaf("Солдат 3"));
            group.Add(new SoldierLeaf("Солдат 4"));
            group.Add(new SoldierLeaf("Солдат 5"));
            group.Add(new SoldierLeaf("Солдат 6"));
            rootComposite.Add(group);

            //відображення дерева строю
            DisplayHierarchy(rootComposite, treeView1.Nodes);

        }
        private void DisplayHierarchy(SoldierComposite composite, TreeNodeCollection nodes)
        {
            TreeNode node = nodes.Add(composite.Name);

            foreach (var person in composite.GetPerson())
            {
                if (person is SoldierLeaf)
                {
                    node.Nodes.Add(person.Name);
                }
                else if (person is SoldierComposite)
                {
                    DisplayHierarchy((SoldierComposite)person, node.Nodes);
                }
            }
        }

        private void ExecuteCommand(ISoldierCommand command)
        {
            command.Execute();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExecuteCommand(new MarchCommand(rootComposite));
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            //виклик команди "Вогонь!"
            ExecuteCommand(new FireCommand(rootComposite));
        }
         private void button3_Click(object sender, EventArgs e)
        {

            //перевіряємо чи там є принаймі один солдат
            if (rootComposite.GetPerson().Count > 0)
            {
                //перевіряємо чи перша людина це SoldierLeaf
                if (rootComposite.GetPerson()[0] is SoldierLeaf firstLeaf)
                {
                    //виконати команду для першого солдата
                    ExecuteCommand(new SingleSoldierCommand(firstLeaf));
                }
                else
                {
                    MessageBox.Show("The first person is not a SoldierLeaf.");
                }
            }
            else
            {
                MessageBox.Show("There are no person in the collection.");
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //виклик команди "Група солдат"
            ExecuteCommand(new GroupCommand((SoldierComposite)rootComposite.GetPerson()[1]));

        }

        //інтерфейс для команди
        interface ISoldierCommand
        {
            void Execute();
        }

        //реалізація команд
        class MarchCommand : ISoldierCommand
        {
            private readonly SoldierComposite target;

            public MarchCommand(SoldierComposite target)
            {
                this.target = target;
            }

            public void Execute()
            {
                MessageBox.Show($"Виконано наказ 'Кроком руш' для {target.Name}");
            }
        }

        class FireCommand : ISoldierCommand
        {
            private readonly SoldierComposite target;

            public FireCommand(SoldierComposite target)
            {
                this.target = target;
            }

            public void Execute()
            {
                MessageBox.Show($"Виконано наказ 'Вогонь!' для {target.Name}");
            }
        }


        //інтерфейс для компонента строю
        interface ISoldierComponent
        {
            string Name { get; }
        }

        //листок композита (Солдат)
        class SoldierLeaf : ISoldierComponent
        {
            public string Name { get; }

            public SoldierLeaf(string name)
            {
                Name = name;
            }
        }

        //композит строю
        class SoldierComposite : ISoldierComponent
        {
            private readonly List<ISoldierComponent> person = new List<ISoldierComponent>();

            public string Name { get; }

            public SoldierComposite(string name)
            {
                Name = name;
            }

            public void Add(ISoldierComponent component)
            {
                person.Add(component);
            }

            public void Remove(ISoldierComponent component)
            {
                person.Remove(component);
            }

            public List<ISoldierComponent> GetPerson()
            {
                return person;
            }
        }


        class SingleSoldierCommand : ISoldierCommand
        {
            private readonly SoldierLeaf target;

            public SingleSoldierCommand(SoldierLeaf target)
            {
                this.target = target;
            }

            public void Execute()
            {
                MessageBox.Show($"Виконано наказ 'Одинокий солдат' для {target.Name}");
            }
        }

        class GroupCommand : ISoldierCommand
        {
            private readonly SoldierComposite target;

            public GroupCommand(SoldierComposite target)
            {
                this.target = target;
            }

            public void Execute()
            {
                MessageBox.Show($"Виконано наказ 'Група солдат' для {target.Name}");
            }
        }



    }
}
