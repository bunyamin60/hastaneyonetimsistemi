namespace HastaneYonetimSistemi
{
    partial class Form1
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnKullaniciEkle = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAdSoyad = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTelefon = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSifre = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbRol = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtEkBilgi1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblEkBilgi1 = new System.Windows.Forms.Label();
            this.lblEkBilgi2 = new System.Windows.Forms.Label();
            this.txtAramaMetni = new System.Windows.Forms.TextBox();
            this.btnKullaniciAra = new System.Windows.Forms.Button();
            this.dgvKullanicilar = new System.Windows.Forms.DataGridView();
            this.txtHastaID = new System.Windows.Forms.TextBox();
            this.txtDoktorID = new System.Windows.Forms.TextBox();
            this.txtIlacID = new System.Windows.Forms.TextBox();
            this.txtMiktar = new System.Windows.Forms.TextBox();
            this.btnReceteEkle = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.textHastaID = new System.Windows.Forms.TextBox();
            this.btnListele = new System.Windows.Forms.Button();
            this.dataGridViewReceteGecmisi = new System.Windows.Forms.DataGridView();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.btnKullaniciSil = new System.Windows.Forms.Button();
            this.btnTestGuncelle = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.cmbEkBilgi2 = new System.Windows.Forms.ComboBox();
            this.txtEkBilgi2 = new System.Windows.Forms.TextBox();
            this.dataGridViewLaboratuvarTestleri = new System.Windows.Forms.DataGridView();
            this.txtTestSonucu = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.btnTestSonucuGuncelle = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKullanicilar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReceteGecmisi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLaboratuvarTestleri)).BeginInit();
            this.SuspendLayout();
            // 
            // btnKullaniciEkle
            // 
            this.btnKullaniciEkle.Location = new System.Drawing.Point(51, 260);
            this.btnKullaniciEkle.Name = "btnKullaniciEkle";
            this.btnKullaniciEkle.Size = new System.Drawing.Size(146, 40);
            this.btnKullaniciEkle.TabIndex = 0;
            this.btnKullaniciEkle.Text = "Kullanıcı Ekle";
            this.btnKullaniciEkle.UseVisualStyleBackColor = true;
            this.btnKullaniciEkle.Click += new System.EventHandler(this.btnKullaniciEkle_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Ad Soyad:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // txtAdSoyad
            // 
            this.txtAdSoyad.Location = new System.Drawing.Point(124, 62);
            this.txtAdSoyad.Name = "txtAdSoyad";
            this.txtAdSoyad.Size = new System.Drawing.Size(100, 22);
            this.txtAdSoyad.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Telefon:";
            // 
            // txtTelefon
            // 
            this.txtTelefon.Location = new System.Drawing.Point(124, 94);
            this.txtTelefon.Name = "txtTelefon";
            this.txtTelefon.Size = new System.Drawing.Size(100, 22);
            this.txtTelefon.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(48, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Şifre:";
            // 
            // txtSifre
            // 
            this.txtSifre.Location = new System.Drawing.Point(124, 123);
            this.txtSifre.Name = "txtSifre";
            this.txtSifre.Size = new System.Drawing.Size(100, 22);
            this.txtSifre.TabIndex = 6;
            this.txtSifre.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(48, 156);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 16);
            this.label4.TabIndex = 7;
            this.label4.Text = "Rol:";
            // 
            // cmbRol
            // 
            this.cmbRol.FormattingEnabled = true;
            this.cmbRol.Items.AddRange(new object[] {
            "Yonetici",
            "Doktor",
            "Hemşire",
            "Hasta"});
            this.cmbRol.Location = new System.Drawing.Point(124, 151);
            this.cmbRol.Name = "cmbRol";
            this.cmbRol.Size = new System.Drawing.Size(100, 24);
            this.cmbRol.TabIndex = 8;
            this.cmbRol.SelectedIndexChanged += new System.EventHandler(this.cmbRol_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(48, 182);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 16);
            this.label5.TabIndex = 9;
            this.label5.Text = "EkBilgi1:";
            // 
            // txtEkBilgi1
            // 
            this.txtEkBilgi1.Location = new System.Drawing.Point(124, 182);
            this.txtEkBilgi1.Name = "txtEkBilgi1";
            this.txtEkBilgi1.Size = new System.Drawing.Size(100, 22);
            this.txtEkBilgi1.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(48, 210);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 16);
            this.label6.TabIndex = 11;
            this.label6.Text = "Ek Bilgi 2:";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // lblEkBilgi1
            // 
            this.lblEkBilgi1.AutoSize = true;
            this.lblEkBilgi1.Location = new System.Drawing.Point(249, 182);
            this.lblEkBilgi1.Name = "lblEkBilgi1";
            this.lblEkBilgi1.Size = new System.Drawing.Size(0, 16);
            this.lblEkBilgi1.TabIndex = 13;
            // 
            // lblEkBilgi2
            // 
            this.lblEkBilgi2.AutoSize = true;
            this.lblEkBilgi2.Location = new System.Drawing.Point(252, 216);
            this.lblEkBilgi2.Name = "lblEkBilgi2";
            this.lblEkBilgi2.Size = new System.Drawing.Size(0, 16);
            this.lblEkBilgi2.TabIndex = 14;
            // 
            // txtAramaMetni
            // 
            this.txtAramaMetni.Location = new System.Drawing.Point(679, 56);
            this.txtAramaMetni.Name = "txtAramaMetni";
            this.txtAramaMetni.Size = new System.Drawing.Size(239, 22);
            this.txtAramaMetni.TabIndex = 15;
            // 
            // btnKullaniciAra
            // 
            this.btnKullaniciAra.Location = new System.Drawing.Point(464, 250);
            this.btnKullaniciAra.Name = "btnKullaniciAra";
            this.btnKullaniciAra.Size = new System.Drawing.Size(214, 44);
            this.btnKullaniciAra.TabIndex = 16;
            this.btnKullaniciAra.Text = "Kullanıcı Ara\r\n";
            this.btnKullaniciAra.UseVisualStyleBackColor = true;
            this.btnKullaniciAra.Click += new System.EventHandler(this.btnKullaniciAra_Click);
            // 
            // dgvKullanicilar
            // 
            this.dgvKullanicilar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKullanicilar.Location = new System.Drawing.Point(464, 94);
            this.dgvKullanicilar.Name = "dgvKullanicilar";
            this.dgvKullanicilar.RowHeadersWidth = 51;
            this.dgvKullanicilar.RowTemplate.Height = 24;
            this.dgvKullanicilar.Size = new System.Drawing.Size(454, 150);
            this.dgvKullanicilar.TabIndex = 17;
            // 
            // txtHastaID
            // 
            this.txtHastaID.Location = new System.Drawing.Point(111, 360);
            this.txtHastaID.Name = "txtHastaID";
            this.txtHastaID.Size = new System.Drawing.Size(113, 22);
            this.txtHastaID.TabIndex = 18;
            // 
            // txtDoktorID
            // 
            this.txtDoktorID.Location = new System.Drawing.Point(111, 388);
            this.txtDoktorID.Name = "txtDoktorID";
            this.txtDoktorID.Size = new System.Drawing.Size(113, 22);
            this.txtDoktorID.TabIndex = 19;
            // 
            // txtIlacID
            // 
            this.txtIlacID.Location = new System.Drawing.Point(111, 416);
            this.txtIlacID.Name = "txtIlacID";
            this.txtIlacID.Size = new System.Drawing.Size(113, 22);
            this.txtIlacID.TabIndex = 20;
            // 
            // txtMiktar
            // 
            this.txtMiktar.Location = new System.Drawing.Point(111, 445);
            this.txtMiktar.Name = "txtMiktar";
            this.txtMiktar.Size = new System.Drawing.Size(113, 22);
            this.txtMiktar.TabIndex = 21;
            // 
            // btnReceteEkle
            // 
            this.btnReceteEkle.Location = new System.Drawing.Point(111, 473);
            this.btnReceteEkle.Name = "btnReceteEkle";
            this.btnReceteEkle.Size = new System.Drawing.Size(113, 33);
            this.btnReceteEkle.TabIndex = 22;
            this.btnReceteEkle.Text = "ReçeteEkle";
            this.btnReceteEkle.UseVisualStyleBackColor = true;
            this.btnReceteEkle.Click += new System.EventHandler(this.btnReceteEkle_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(108, 341);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(103, 16);
            this.label7.TabIndex = 23;
            this.label7.Text = " Reçete Ekleme";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(48, 363);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 16);
            this.label8.TabIndex = 24;
            this.label8.Text = "HastaID:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(44, 391);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 16);
            this.label9.TabIndex = 25;
            this.label9.Text = "DoktorID:";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(63, 419);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(44, 16);
            this.label10.TabIndex = 26;
            this.label10.Text = "İlaçID:";
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(61, 448);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(46, 16);
            this.label11.TabIndex = 27;
            this.label11.Text = "Miktar:";
            // 
            // textHastaID
            // 
            this.textHastaID.Location = new System.Drawing.Point(424, 332);
            this.textHastaID.Name = "textHastaID";
            this.textHastaID.Size = new System.Drawing.Size(177, 22);
            this.textHastaID.TabIndex = 28;
            // 
            // btnListele
            // 
            this.btnListele.Location = new System.Drawing.Point(409, 512);
            this.btnListele.Name = "btnListele";
            this.btnListele.Size = new System.Drawing.Size(136, 39);
            this.btnListele.TabIndex = 29;
            this.btnListele.Text = "Reçete Bul\r\n";
            this.btnListele.UseVisualStyleBackColor = true;
            this.btnListele.Click += new System.EventHandler(this.btnListele_Click);
            // 
            // dataGridViewReceteGecmisi
            // 
            this.dataGridViewReceteGecmisi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewReceteGecmisi.Location = new System.Drawing.Point(361, 356);
            this.dataGridViewReceteGecmisi.Name = "dataGridViewReceteGecmisi";
            this.dataGridViewReceteGecmisi.RowHeadersWidth = 51;
            this.dataGridViewReceteGecmisi.RowTemplate.Height = 24;
            this.dataGridViewReceteGecmisi.Size = new System.Drawing.Size(240, 150);
            this.dataGridViewReceteGecmisi.TabIndex = 30;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(421, 297);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(94, 16);
            this.label12.TabIndex = 31;
            this.label12.Text = "Reçete Bulucu\r\n";
            this.label12.Click += new System.EventHandler(this.label12_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(358, 335);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(62, 16);
            this.label13.TabIndex = 32;
            this.label13.Text = "HastaID :";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(461, 59);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(198, 16);
            this.label14.TabIndex = 33;
            this.label14.Text = "Bir kullanıcı rolü ya da adı giriniz:";
            this.label14.Click += new System.EventHandler(this.label14_Click);
            // 
            // btnKullaniciSil
            // 
            this.btnKullaniciSil.Location = new System.Drawing.Point(684, 250);
            this.btnKullaniciSil.Name = "btnKullaniciSil";
            this.btnKullaniciSil.Size = new System.Drawing.Size(234, 44);
            this.btnKullaniciSil.TabIndex = 34;
            this.btnKullaniciSil.Text = "Kullanıcı Sİl";
            this.btnKullaniciSil.UseVisualStyleBackColor = true;
            this.btnKullaniciSil.Click += new System.EventHandler(this.btnKullaniciSil_Click);
            // 
            // btnTestGuncelle
            // 
            this.btnTestGuncelle.Location = new System.Drawing.Point(0, 0);
            this.btnTestGuncelle.Name = "btnTestGuncelle";
            this.btnTestGuncelle.Size = new System.Drawing.Size(75, 23);
            this.btnTestGuncelle.TabIndex = 45;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(721, 322);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(184, 16);
            this.label15.TabIndex = 42;
            this.label15.Text = "Laboratuvar Test Güncelleme";
            // 
            // cmbEkBilgi2
            // 
            this.cmbEkBilgi2.FormattingEnabled = true;
            this.cmbEkBilgi2.Location = new System.Drawing.Point(124, 210);
            this.cmbEkBilgi2.Name = "cmbEkBilgi2";
            this.cmbEkBilgi2.Size = new System.Drawing.Size(100, 24);
            this.cmbEkBilgi2.TabIndex = 43;
            // 
            // txtEkBilgi2
            // 
            this.txtEkBilgi2.Location = new System.Drawing.Point(124, 210);
            this.txtEkBilgi2.Name = "txtEkBilgi2";
            this.txtEkBilgi2.Size = new System.Drawing.Size(100, 22);
            this.txtEkBilgi2.TabIndex = 44;
            this.txtEkBilgi2.TextChanged += new System.EventHandler(this.txtEkBilgi2_TextChanged);
            // 
            // dataGridViewLaboratuvarTestleri
            // 
            this.dataGridViewLaboratuvarTestleri.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewLaboratuvarTestleri.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLaboratuvarTestleri.Location = new System.Drawing.Point(659, 356);
            this.dataGridViewLaboratuvarTestleri.Name = "dataGridViewLaboratuvarTestleri";
            this.dataGridViewLaboratuvarTestleri.ReadOnly = true;
            this.dataGridViewLaboratuvarTestleri.RowHeadersWidth = 51;
            this.dataGridViewLaboratuvarTestleri.RowTemplate.Height = 24;
            this.dataGridViewLaboratuvarTestleri.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewLaboratuvarTestleri.Size = new System.Drawing.Size(327, 150);
            this.dataGridViewLaboratuvarTestleri.TabIndex = 46;
            // 
            // txtTestSonucu
            // 
            this.txtTestSonucu.Location = new System.Drawing.Point(816, 512);
            this.txtTestSonucu.Name = "txtTestSonucu";
            this.txtTestSonucu.Size = new System.Drawing.Size(170, 22);
            this.txtTestSonucu.TabIndex = 47;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(647, 518);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(154, 16);
            this.label16.TabIndex = 48;
            this.label16.Text = "Yeni Test Sonucu Giriniz:";
            this.label16.Click += new System.EventHandler(this.label16_Click);
            // 
            // btnTestSonucuGuncelle
            // 
            this.btnTestSonucuGuncelle.Location = new System.Drawing.Point(684, 540);
            this.btnTestSonucuGuncelle.Name = "btnTestSonucuGuncelle";
            this.btnTestSonucuGuncelle.Size = new System.Drawing.Size(262, 36);
            this.btnTestSonucuGuncelle.TabIndex = 49;
            this.btnTestSonucuGuncelle.Text = "GÜNCELLE";
            this.btnTestSonucuGuncelle.UseVisualStyleBackColor = true;
            this.btnTestSonucuGuncelle.Click += new System.EventHandler(this.btnTestSonucuGuncelle_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(998, 638);
            this.Controls.Add(this.btnTestSonucuGuncelle);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.txtTestSonucu);
            this.Controls.Add(this.dataGridViewLaboratuvarTestleri);
            this.Controls.Add(this.txtEkBilgi2);
            this.Controls.Add(this.cmbEkBilgi2);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.btnTestGuncelle);
            this.Controls.Add(this.btnKullaniciSil);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.dataGridViewReceteGecmisi);
            this.Controls.Add(this.btnListele);
            this.Controls.Add(this.textHastaID);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnReceteEkle);
            this.Controls.Add(this.txtMiktar);
            this.Controls.Add(this.txtIlacID);
            this.Controls.Add(this.txtDoktorID);
            this.Controls.Add(this.txtHastaID);
            this.Controls.Add(this.dgvKullanicilar);
            this.Controls.Add(this.btnKullaniciAra);
            this.Controls.Add(this.txtAramaMetni);
            this.Controls.Add(this.lblEkBilgi2);
            this.Controls.Add(this.lblEkBilgi1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtEkBilgi1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbRol);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSifre);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtTelefon);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtAdSoyad);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnKullaniciEkle);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKullanicilar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReceteGecmisi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLaboratuvarTestleri)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnKullaniciEkle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAdSoyad;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTelefon;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSifre;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbRol;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtEkBilgi1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblEkBilgi1;
        private System.Windows.Forms.Label lblEkBilgi2;
        private System.Windows.Forms.TextBox txtAramaMetni;
        private System.Windows.Forms.Button btnKullaniciAra;
        private System.Windows.Forms.DataGridView dgvKullanicilar;
        private System.Windows.Forms.TextBox txtHastaID;
        private System.Windows.Forms.TextBox txtDoktorID;
        private System.Windows.Forms.TextBox txtIlacID;
        private System.Windows.Forms.TextBox txtMiktar;
        private System.Windows.Forms.Button btnReceteEkle;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textHastaID;
        private System.Windows.Forms.Button btnListele;
        private System.Windows.Forms.DataGridView dataGridViewReceteGecmisi;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnKullaniciSil;
        private System.Windows.Forms.Button btnTestGuncelle;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox cmbEkBilgi2;
        private System.Windows.Forms.TextBox txtEkBilgi2;
        private System.Windows.Forms.DataGridView dataGridViewLaboratuvarTestleri;
        private System.Windows.Forms.TextBox txtTestSonucu;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button btnTestSonucuGuncelle;
    }
}

