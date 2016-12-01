namespace WindowsFormsApplication3
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.открытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.данныеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.спроецироватьВСобственноеПодпространствоToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.стандартизацияПеременныхToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.классификацияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.данныеПоДвумГруппамToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button2 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.KmeansBtn = new System.Windows.Forms.Button();
            this.ForelBtn = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button7 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.IerarhicalClasterization = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.срВзвешРасстояниеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(500, 500);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(509, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 48);
            this.button1.TabIndex = 1;
            this.button1.Text = "Скрыть/показать не выделенные точки";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(-4, 6);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1093, 500);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(531, 105);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(107, 34);
            this.button3.TabIndex = 4;
            this.button3.Text = "Построить матрицу";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(531, 60);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(107, 39);
            this.button4.TabIndex = 5;
            this.button4.Text = "Снять выделения с точек";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.данныеToolStripMenuItem,
            this.классификацияToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1316, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.открытьToolStripMenuItem,
            this.выходToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(48, 20);
            this.toolStripMenuItem1.Text = "Файл";
            // 
            // открытьToolStripMenuItem
            // 
            this.открытьToolStripMenuItem.Name = "открытьToolStripMenuItem";
            this.открытьToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.открытьToolStripMenuItem.Text = "Открыть";
            this.открытьToolStripMenuItem.Click += new System.EventHandler(this.открытьToolStripMenuItem_Click);
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.выходToolStripMenuItem.Text = "Выход";
            this.выходToolStripMenuItem.Click += new System.EventHandler(this.выходToolStripMenuItem_Click);
            // 
            // данныеToolStripMenuItem
            // 
            this.данныеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.спроецироватьВСобственноеПодпространствоToolStripMenuItem,
            this.стандартизацияПеременныхToolStripMenuItem});
            this.данныеToolStripMenuItem.Name = "данныеToolStripMenuItem";
            this.данныеToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.данныеToolStripMenuItem.Text = "Данные";
            // 
            // спроецироватьВСобственноеПодпространствоToolStripMenuItem
            // 
            this.спроецироватьВСобственноеПодпространствоToolStripMenuItem.Name = "спроецироватьВСобственноеПодпространствоToolStripMenuItem";
            this.спроецироватьВСобственноеПодпространствоToolStripMenuItem.Size = new System.Drawing.Size(341, 22);
            this.спроецироватьВСобственноеПодпространствоToolStripMenuItem.Text = "Спроецировать в собственное подпространство";
            this.спроецироватьВСобственноеПодпространствоToolStripMenuItem.Click += new System.EventHandler(this.спроецироватьВСобственноеПодпространствоToolStripMenuItem_Click);
            // 
            // стандартизацияПеременныхToolStripMenuItem
            // 
            this.стандартизацияПеременныхToolStripMenuItem.Name = "стандартизацияПеременныхToolStripMenuItem";
            this.стандартизацияПеременныхToolStripMenuItem.Size = new System.Drawing.Size(341, 22);
            this.стандартизацияПеременныхToolStripMenuItem.Text = "Стандартизация переменных";
            this.стандартизацияПеременныхToolStripMenuItem.Click += new System.EventHandler(this.стандартизацияПеременныхToolStripMenuItem_Click);
            // 
            // классификацияToolStripMenuItem
            // 
            this.классификацияToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.данныеПоДвумГруппамToolStripMenuItem,
            this.срВзвешРасстояниеToolStripMenuItem});
            this.классификацияToolStripMenuItem.Name = "классификацияToolStripMenuItem";
            this.классификацияToolStripMenuItem.Size = new System.Drawing.Size(106, 20);
            this.классификацияToolStripMenuItem.Text = "Классификация";
            // 
            // данныеПоДвумГруппамToolStripMenuItem
            // 
            this.данныеПоДвумГруппамToolStripMenuItem.Name = "данныеПоДвумГруппамToolStripMenuItem";
            this.данныеПоДвумГруппамToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
            this.данныеПоДвумГруппамToolStripMenuItem.Text = "Расстояние между проекциями";
            this.данныеПоДвумГруппамToolStripMenuItem.Click += new System.EventHandler(this.данныеПоДвумГруппамToolStripMenuItem_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(531, 145);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(107, 43);
            this.button2.TabIndex = 7;
            this.button2.Text = "Добавить кластер";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(531, 194);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(107, 82);
            this.listBox1.TabIndex = 8;
            this.listBox1.Click += new System.EventHandler(this.listBox1_Click);
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.White;
            this.pictureBox2.Location = new System.Drawing.Point(661, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(675, 500);
            this.pictureBox2.TabIndex = 9;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseClick);
            this.pictureBox2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseDown);
            this.pictureBox2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseMove);
            this.pictureBox2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseUp);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // KmeansBtn
            // 
            this.KmeansBtn.Location = new System.Drawing.Point(531, 367);
            this.KmeansBtn.Name = "KmeansBtn";
            this.KmeansBtn.Size = new System.Drawing.Size(86, 23);
            this.KmeansBtn.TabIndex = 10;
            this.KmeansBtn.Text = "K means";
            this.KmeansBtn.UseVisualStyleBackColor = true;
            this.KmeansBtn.Click += new System.EventHandler(this.KmeansBtn_Click);
            // 
            // ForelBtn
            // 
            this.ForelBtn.Location = new System.Drawing.Point(749, 519);
            this.ForelBtn.Name = "ForelBtn";
            this.ForelBtn.Size = new System.Drawing.Size(75, 40);
            this.ForelBtn.TabIndex = 11;
            this.ForelBtn.Text = "Forel";
            this.ForelBtn.UseVisualStyleBackColor = true;
            this.ForelBtn.Visible = false;
            this.ForelBtn.Click += new System.EventHandler(this.ForelBtn_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(580, 284);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(51, 20);
            this.textBox1.TabIndex = 12;
            this.textBox1.Text = "2";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(531, 405);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(86, 52);
            this.button5.TabIndex = 13;
            this.button5.Text = "Очистить списки кластеров";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(531, 310);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(86, 42);
            this.button6.TabIndex = 14;
            this.button6.Text = "выбор нач точек";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(520, 288);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Lp metrics";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(945, 535);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(118, 23);
            this.button7.TabIndex = 16;
            this.button7.Text = "плотность на точках\r\n";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Visible = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(1030, 509);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(33, 20);
            this.textBox2.TabIndex = 17;
            this.textBox2.Text = "5";
            this.textBox2.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(960, 519);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "интервалов";
            this.label2.Visible = false;
            // 
            // IerarhicalClasterization
            // 
            this.IerarhicalClasterization.Location = new System.Drawing.Point(830, 519);
            this.IerarhicalClasterization.Name = "IerarhicalClasterization";
            this.IerarhicalClasterization.Size = new System.Drawing.Size(104, 40);
            this.IerarhicalClasterization.TabIndex = 20;
            this.IerarhicalClasterization.Text = "иерархическая кластеризация";
            this.IerarhicalClasterization.UseVisualStyleBackColor = true;
            this.IerarhicalClasterization.Click += new System.EventHandler(this.Ierarhic_Clasteriz);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(580, 6);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(75, 48);
            this.button10.TabIndex = 21;
            this.button10.Text = "Убрать точки кластера";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(661, 519);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(82, 40);
            this.button8.TabIndex = 22;
            this.button8.Text = "обновиться";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(531, 463);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(86, 40);
            this.button9.TabIndex = 23;
            this.button9.Text = "Профильная диаграмма";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(0, 20);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1329, 619);
            this.tabControl1.TabIndex = 24;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridView1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1321, 593);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Табличный вид данных";
            this.tabPage2.UseVisualStyleBackColor = true;
            this.tabPage2.Click += new System.EventHandler(this.tabPage2_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Controls.Add(this.button9);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.button8);
            this.tabPage1.Controls.Add(this.IerarhicalClasterization);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.button10);
            this.tabPage1.Controls.Add(this.textBox2);
            this.tabPage1.Controls.Add(this.button3);
            this.tabPage1.Controls.Add(this.button7);
            this.tabPage1.Controls.Add(this.button4);
            this.tabPage1.Controls.Add(this.listBox1);
            this.tabPage1.Controls.Add(this.pictureBox2);
            this.tabPage1.Controls.Add(this.KmeansBtn);
            this.tabPage1.Controls.Add(this.ForelBtn);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.textBox1);
            this.tabPage1.Controls.Add(this.button6);
            this.tabPage1.Controls.Add(this.button5);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1321, 593);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Построение";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // срВзвешРасстояниеToolStripMenuItem
            // 
            this.срВзвешРасстояниеToolStripMenuItem.Name = "срВзвешРасстояниеToolStripMenuItem";
            this.срВзвешРасстояниеToolStripMenuItem.Size = new System.Drawing.Size(248, 22);
            this.срВзвешРасстояниеToolStripMenuItem.Text = "СрВзвеш расстояние";
            this.срВзвешРасстояниеToolStripMenuItem.Click += new System.EventHandler(this.срВзвешРасстояниеToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1316, 621);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem открытьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button KmeansBtn;
        private System.Windows.Forms.Button ForelBtn;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button IerarhicalClasterization;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ToolStripMenuItem данныеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem спроецироватьВСобственноеПодпространствоToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem стандартизацияПеременныхToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem классификацияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem данныеПоДвумГруппамToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem срВзвешРасстояниеToolStripMenuItem;
    }
}

