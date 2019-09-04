using ControlePJS.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using static ControlePJS.Database;

namespace ControlePJS
{
    public partial class Form1 : Form
    {
        public static List<Projeto> ListaProjetos = Executar.AbrirBanco<Projeto>(Settings.Default.Book);
        public static int EmEdicao;
        public Form1()
        {
            InitializeComponent();
        }      


        //Procedimentos de Projeto
        void SalvarAlterações()
        {
            var group = this.Controls.Find("proj" + EmEdicao.ToString(), true);
            var panel1 = this.Controls.Find("panel1", true);
            var panel2 = this.Controls.Find("panel2", true);
            var panel3 = this.Controls.Find("panel3", true);

            var proj = ListaProjetos.First(p => p.ID == EmEdicao);
            proj.Nome = boxNome.Text;
            proj.Cliente = boxCliente.Text;
            proj.Descricao = boxDesc.Text;
            proj.Entrega = boxData.Value;
            proj.Recebido = Convert.ToInt32(boxRecebido.Text);
            proj.Total = Convert.ToInt32(boxTotal.Text);
            proj.Status = boxStatus.SelectedIndex;

            group[0].Text = proj.Nome + " / " + proj.Cliente;
            var lbDesc = group[0].Controls.Find("lbDesc" + EmEdicao.ToString(), true);
            lbDesc[0].Text = proj.Descricao;
            var lbRecebido = group[0].Controls.Find("lbRecebido" + EmEdicao.ToString(), true)[0];
            lbRecebido.Text = "R$ " + boxRecebido.Text;
            var lbEntrega = group[0].Controls.Find("lbEntrega" + EmEdicao.ToString(), true)[0];
            lbEntrega.Text = proj.Entrega.ToShortDateString();
            var lbValor = group[0].Controls.Find("lbValor" + EmEdicao.ToString(), true)[0];
            lbValor.Text = "R$ " + boxTotal.Text;
            LimpaForm();
            btnSalvar.Visible = false;
            btnRemover.Visible = false;
            AtualizaContagem();
            Executar.SalvarBanco<Projeto>(ListaProjetos, Settings.Default.Book);

        }
        void RemoverProjeto(int ID)
        {
            var group = this.Controls.Find("proj" + ID.ToString(), true);
            var panel1 = this.Controls.Find("panel1", true);
            var panel2 = this.Controls.Find("panel2", true);
            var panel3 = this.Controls.Find("panel3", true);
            panel1[0].Controls.Remove(group[0]);
            panel2[0].Controls.Remove(group[0]);
            panel3[0].Controls.Remove(group[0]);
            ListaProjetos.Remove(ListaProjetos.First(p => p.ID == ID));
            LimpaForm();

            btnSalvar.Visible = false;
            btnRemover.Visible = false;
            AtualizaContagem();
            Executar.SalvarBanco<Projeto>(ListaProjetos, Settings.Default.Book);

        }
        void InserirProjeto()
        {
            int id = 0;
            while (ListaProjetos.FindAll(p => p.ID == id).Count != 0)
            {
                id++;
            }

            var proj = new Projeto
            {
                Cliente = boxCliente.Text,
                DataInicio = DateTime.Now,
                Descricao = boxDesc.Text,
                Entrega = boxData.Value,
                Nome = boxNome.Text,
                Status = boxStatus.SelectedIndex,
                ID = id,
                Recebido = Convert.ToInt32(boxRecebido.Text),
                Total = Convert.ToInt32(boxTotal.Text),
            };

            InserirItemProj(proj);
            ListaProjetos.Add(proj);
            AtualizaContagem();
            Executar.SalvarBanco<Projeto>(ListaProjetos, Settings.Default.Book);
            LimpaForm();

        }
        void PopularProjetos()
        {
            foreach (var item in ListaProjetos)
            {
                InserirItemProj(item);
            }
            AtualizaContagem();
        }
        void InserirItemProj(Projeto proj)
        {
            var panel1 = this.Controls.Find("panel1", true);
            var panel2 = this.Controls.Find("panel2", true);
            var panel3 = this.Controls.Find("panel3", true);

            GroupBox nGroup = new GroupBox();
            nGroup.Name = "proj" + proj.ID.ToString();
            nGroup.ForeColor = Color.OrangeRed;
            nGroup.Size = new Size(249, 132);
            nGroup.Text = proj.Nome + " / " + proj.Cliente;
            nGroup.Dock = DockStyle.Top;
            nGroup.Location = new Point(0, 0);

            Label lbValor = new Label();
            lbValor.Location = new Point(145, 16);
            lbValor.Name = "lbValor" + proj.ID.ToString();
            lbValor.Text = "R$ " + proj.Total.ToString();
            lbValor.Font = new Font("Microsoft Sans Serif", 15);
            nGroup.Controls.Add(lbValor);


            Label lbEntrega = new Label();
            lbEntrega.Location = new Point(145, 46);
            lbEntrega.Text = "Entrega:";
            lbEntrega.AutoSize = false;
            lbEntrega.Font = new Font("Microsoft Sans Serif", 8);

            Label lbData = new Label();
            lbData.Location = new Point(145, 59);
            lbData.Name = "lbEntrega" + proj.ID.ToString();
            lbData.Text = proj.Entrega.ToShortDateString();
            lbData.AutoSize = false;
            lbData.Font = new Font("Microsoft Sans Serif", 10);
            nGroup.Controls.Add(lbData);
            nGroup.Controls.Add(lbEntrega);


            Label lbRecebido = new Label();
            lbRecebido.Location = new Point(145, 84);
            lbRecebido.Text = "Recebido:";
            lbRecebido.AutoSize = false;
            lbRecebido.Font = new Font("Microsoft Sans Serif", 8);

            Label lbRecebidoV = new Label();
            lbRecebidoV.Location = new Point(145, 97);
            lbRecebidoV.Name = "lbRecebido" + proj.ID.ToString();
            lbRecebidoV.Text = "R$ " + proj.Recebido.ToString();
            lbRecebidoV.AutoSize = false;
            lbRecebidoV.Font = new Font("Microsoft Sans Serif", 10);
            nGroup.Controls.Add(lbRecebidoV);
            nGroup.Controls.Add(lbRecebido);

            Label lbDesc = new Label();
            lbDesc.Location = new Point(16, 33);
            lbDesc.Size = new Size(117, 57);
            lbDesc.AutoSize = false;
            lbDesc.Name = "lbDesc" + proj.ID.ToString();
            lbDesc.Text = proj.Descricao;
            lbDesc.Font = new Font("Microsoft Sans Serif", 8);
            nGroup.Controls.Add(lbDesc);

            PictureBox right = new PictureBox();
            right.Location = new Point(85, 93);
            right.Cursor = Cursors.Hand;
            right.Size = new Size(24, 24);
            right.Image = Properties.Resources.right_arrow;
            right.Click += Esquerda;
            right.Name = proj.ID.ToString();
            nGroup.Controls.Add(right);

            PictureBox left = new PictureBox();
            left.Location = new Point(45, 93);
            left.Size = new Size(24, 24);
            left.Cursor = Cursors.Hand;
            left.Click += Direita;
            left.Name = proj.ID.ToString();
            left.Image = Properties.Resources.left_arrow;
            nGroup.Controls.Add(left);


            PictureBox edit = new PictureBox();
            edit.Location = new Point(12, 92);
            edit.Size = new Size(24, 24);
            edit.Cursor = Cursors.Hand;
            edit.Click += Editar;
            edit.Name = proj.ID.ToString();
            edit.Image = Properties.Resources.edit;
            nGroup.Controls.Add(edit);

            if (proj.Status == 0)
            {
                nGroup.ForeColor = Color.OrangeRed;
                panel1[0].Controls.Add(nGroup);
            }
            if (proj.Status == 1)
            {
                nGroup.ForeColor = Color.SteelBlue;
                panel2[0].Controls.Add(nGroup);
            }
            if (proj.Status == 2)
            {
                nGroup.ForeColor = Color.SeaGreen;
                panel3[0].Controls.Add(nGroup);
            }
        }


        //Procedimentos de Book
        void RemoverBook(string nome)
        {
            var group = this.Controls.Find("book" + nome, true);
            var panel4 = this.Controls.Find("panel4", true);
            panel4[0].Controls.Remove(group[0]);

            if (File.Exists("beta/" + nome + ".proj")) { File.Delete("beta/" + nome + ".proj"); }
        }
        void PopularBooks()
        {
            foreach(var arq in Directory.GetFiles(Environment.CurrentDirectory + "/beta/"))
            {
                InserirItemBook(arq.Replace(Environment.CurrentDirectory, "").Replace("/beta/","").Replace(".proj",""));
            }
        }
        void AbrirBook(string nome)
        {
            Executar.SalvarBanco<Projeto>(ListaProjetos, Settings.Default.Book);
            Settings.Default.Book = nome;
            Settings.Default.Save();


            foreach (var proj in ListaProjetos)
            {
                var group = this.Controls.Find("proj" + proj.ID.ToString(), true);
                var panel1 = this.Controls.Find("panel1", true);
                var panel2 = this.Controls.Find("panel2", true);
                var panel3 = this.Controls.Find("panel3", true);
                panel1[0].Controls.Remove(group[0]);
                panel2[0].Controls.Remove(group[0]);
                panel3[0].Controls.Remove(group[0]);
            }

            ListaProjetos.Clear();
            AtualizaContagem();

            ListaProjetos = Executar.AbrirBanco<Projeto>(Settings.Default.Book);
            PopularProjetos();
        }
        void InserirItemBook(string nome)
        {
            var panel4 = this.Controls.Find("panel4", true);

            GroupBox nGroup = new GroupBox();
            nGroup.Name = "book" + nome;
            nGroup.ForeColor = Color.DarkBlue;
            nGroup.Size = new Size(280, 57);
            nGroup.Text = nome;
            nGroup.Dock = DockStyle.Top;

            Label lbDesc = new Label();
            lbDesc.Location = new Point(178, 24);
            lbDesc.Size = new Size(56, 13);
            lbDesc.AutoSize = false;
            lbDesc.Text = "Abrir Book";
            lbDesc.Font = new Font("Microsoft Sans Serif", 8);
            nGroup.Controls.Add(lbDesc);

            PictureBox abrir = new PictureBox();
            abrir.Location = new Point(240, 19);
            abrir.Cursor = Cursors.Hand;
            abrir.Name = nome;
            abrir.Size = new Size(24, 24);
            abrir.Image = Properties.Resources.right_arrow;
            abrir.Click += AbrirClick;
            nGroup.Controls.Add(abrir);


            PictureBox deletar = new PictureBox();
            deletar.Location = new Point(9, 19);
            deletar.Cursor = Cursors.Hand;
            deletar.Size = new Size(24, 24);
            deletar.Name = nome;
            deletar.Image = Properties.Resources.delete;
            deletar.Click += RemoverBookClick;
            nGroup.Controls.Add(deletar);

            panel4[0].Controls.Add(nGroup);
        }


        //Procedimentos Auxiliares
        void LimpaForm()
        {

            boxCliente.Text = "";
            boxNome.Text = "";
            boxDesc.Text = "";
            boxRecebido.Text = "";
            boxStatus.Text = "";
            boxTotal.Text = "";
            boxData.Value = DateTime.Now;
        }
        void ReLocalizaProjeto(int id, bool left)
        {
            var proj = ListaProjetos.First(p => p.ID == id);
            var group = this.Controls.Find("proj" + proj.ID.ToString(), true);
            var panel1 = this.Controls.Find("panel1", true);
            var panel2 = this.Controls.Find("panel2", true);
            var panel3 = this.Controls.Find("panel3", true);

            if (left && proj.Status > 0)
            {
                proj.Status--;
            }
            else if (!left && proj.Status < 2)
            {
                proj.Status++;
            }

            if (proj.Status == 0)
            {
                group[0].ForeColor = Color.OrangeRed;
                group[0].Parent = panel1[0];
            }
            if (proj.Status == 1)
            {
                group[0].ForeColor = Color.SteelBlue;
                group[0].Parent = panel2[0];
            }
            if (proj.Status == 2)
            {
                group[0].ForeColor = Color.SeaGreen;
                group[0].Parent = panel3[0];
            }
            Executar.SalvarBanco<Projeto>(ListaProjetos, Settings.Default.Book);

        }
        void AtualizaContagem()
        {
            groupDesenvolvimento.Text = "Projetos em Desenvolvimento (" + ListaProjetos.FindAll(p => p.Status == 0).Count.ToString() + ")";
            groupTeste.Text = "Projetos em Teste (" + ListaProjetos.FindAll(p => p.Status == 1).Count.ToString() + ")";
            groupFinalizados.Text = "Projetos Finalizados (" + ListaProjetos.FindAll(p => p.Status == 2).Count.ToString() + ")";

            lbProjetos.Text = ListaProjetos.Count().ToString();

            int receber = 0;
            int recebido = 0;
            foreach (var item in ListaProjetos)
            {
                recebido = recebido + item.Recebido;
                receber = receber + item.Total - item.Recebido;
            }
            lbReceber.Text = receber.ToString();
            lbRecebido.Text = recebido.ToString();

            lbTotal.Text = (receber + recebido).ToString();
        }



        private void PictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            if(boxNome.Text == "" ||
                boxRecebido.Text == "" ||
                boxTotal.Text == "") { MessageBox.Show("Alguns campos não podem ficar em branco.");return; }

            SalvarAlterações();
        }
        private void BtnRemover_Click(object sender, EventArgs e)
        {
            RemoverProjeto(EmEdicao);
        }
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (boxNome.Text == "" ||
                boxRecebido.Text == "" ||
                boxTotal.Text == "") { MessageBox.Show("Alguns campos não podem ficar em branco."); return; }
            InserirProjeto();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            PopularProjetos();
            PopularBooks();
        }
      
        private void AbrirClick(object sender, EventArgs e)
        {
            PictureBox s = sender as PictureBox;
            AbrirBook(s.Name);
        }
        void RemoverBookClick(object sender, EventArgs e)
        {
            PictureBox s = sender as PictureBox;

            RemoverBook(s.Name);
        }
        private void Direita(object sender, EventArgs e)
        {
            var btn = sender as PictureBox;
            ReLocalizaProjeto(Convert.ToInt32(btn.Name), true);
            AtualizaContagem();
        }
        private void Esquerda(object sender, EventArgs e)
        {
            var btn = sender as PictureBox;
            ReLocalizaProjeto(Convert.ToInt32(btn.Name), false);
            AtualizaContagem();
        }
        private void Editar(object sender, EventArgs e)
        {
            var btn = sender as PictureBox;
            var proj = ListaProjetos.First(p => p.ID == Convert.ToInt32(btn.Name));
            boxNome.Text = proj.Nome;
            boxCliente.Text = proj.Cliente;
            boxDesc.Text = proj.Descricao;
            boxData.Value = proj.Entrega;
            boxRecebido.Text = proj.Recebido.ToString();
            boxStatus.SelectedIndex = proj.Status;
            boxTotal.Text = proj.Total.ToString();
            EmEdicao = proj.ID;
            btnSalvar.Visible = true;
            btnRemover.Visible = true;
        }

        private void BtnNovoBook_Click(object sender, EventArgs e)
        {

            if (File.Exists("beta/" + boxBook.Text + ".proj") || boxBook.Text == "") { MessageBox.Show("Não é possível usar esse nome."); return; }
            InserirItemBook(boxBook.Text);
            AbrirBook(boxBook.Text);
        }

        private void BoxRecebido_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
            {

                e.Handled = true;

            }
        }

        private void BoxTotal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
            {

                e.Handled = true;

            }
        }
    }
}
