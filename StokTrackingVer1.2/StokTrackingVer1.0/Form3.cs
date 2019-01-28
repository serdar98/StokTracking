using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;//sql veri tabanı için
using iTextSharp.text;//raporlama için
using iTextSharp.text.pdf;//raporlamayı pdf e çevirmek için
using System.IO;


namespace StokTrackingVer1._0
{
    public partial class Form3 : Form
    {
      
        //veritabanı bağlantısı için gerekli sürücü ve parametreler tanımlandı
        SqlConnection connection;
        SqlDataAdapter sqlDataAdapter;
        SqlCommand sqlCommand;
        DataSet dataSet;

        int selectedRow;

        public Form3()
        {
            InitializeComponent();
        }

        private void btnAddStock_Click(object sender, EventArgs e)
        {
            //SqlCommand ile yeni bir komut göndermek için hazırlık yapıyoruz
            sqlCommand = new SqlCommand();

            //bağlantı açılır
            connection.Open();

            //komutun hangi bağlantı yoluyla çalıştırılacağı belirtilir
            sqlCommand.Connection = connection;

            //insert into SQL komutuyla veritabanındaki tabloya form verilerini parametreli şekilde göndeririz
            //@ işaretiyle başlayanlar parametreleri belirtir.
            sqlCommand.CommandText = "SET IDENTITY_INSERT Kayıt ON insert into Kayıt(adı,model,adet,seriNo,tarih,kayıtYapan)values(@pAdi,@pModel,@pAdet,@pSerino,@pTarih,@pKayitYapan)";

            sqlCommand.Parameters.AddWithValue("pAdi", txtName.Text);
            sqlCommand.Parameters.AddWithValue("pModel", txtModel.Text);
            sqlCommand.Parameters.AddWithValue("pAdet", txtPiece.Text);
            sqlCommand.Parameters.AddWithValue("pSerino", txtSerialNo.Text);
            sqlCommand.Parameters.AddWithValue("pTarih", dtDate.Value);
            sqlCommand.Parameters.AddWithValue("pKayitYapan", txtSave.Text);

            //komutu çalıştırıyoruz
            sqlCommand.ExecuteNonQuery();

            //baglanti yı kapatıyoruz
            connection.Close();
            
            //eklediğimiz verilerin gösterilmesi için  dataFiiling() metodunu yineliyoruz
            dataFiiling();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            dataFiiling();
            UPDATE();
           
        }

        public void dataFiiling()
        {
            connection = new SqlConnection("server=.; Initial Catalog=StokTakip;Integrated Security=true");
            sqlDataAdapter = new SqlDataAdapter("Select*from Kayıt", connection);
            dataSet = new DataSet();

            connection.Open();
            sqlDataAdapter.Fill(dataSet, "Kayıt");
            dataGridView1.DataSource = dataSet.Tables["Kayıt"];
            connection.Close();

        }

        private void btnDeleteStock_Click(object sender, EventArgs e)
        {
            connection.Open();
            string selectQuery = "SELECT * from Kayıt where seriNo=@pSerino";

            //serialno parametresine bağlı olarak seri nnumarasını çekiyoruz
            SqlCommand selectCommand = new SqlCommand(selectQuery, connection);
            selectCommand.Parameters.AddWithValue("@pSerino", txtSerialNo.Text);

            //serialno parametremize textbox'dan girilen değeri aktarıyoruz.
            SqlDataAdapter dataAdapter = new SqlDataAdapter(selectCommand);
            SqlDataReader dataReader = selectCommand.ExecuteReader();

            //DataReader ile serialno'yu veritabanından belleğe aktarıyoruz.
            if (dataReader.Read()) 
            {
                string SerialNo = dataReader["adı"].ToString(); //Datareader ile okunan serialno'yu serialNo değişkenine atıyoruz
                dataReader.Close(); //Datareader açık olduğu sürece başka bir sorgu çalıştıramayacağımız için dataReader nesnesini kapatıyoruz.

                DialogResult result = MessageBox.Show(SerialNo + " kaydını silmek istediğinizden emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo);
                //Kullanıcıya silme onayı penceresi açıp, verdiği cevabı result değişkenine aktardık.

                if (DialogResult.Yes == result) // Eğer kullanıcı Evet seçeneğini seçmişse, veritabanından kaydı silecek kodlar çalışır.
                {
                    string deleteQuery = "DELETE from Kayıt where seriNo=@pSerino"; //serialno parametresine bağlı olarak stok kaydını silen sql sorgusu
                    SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection);
                    deleteCommand.Parameters.AddWithValue("@pSerino", txtSerialNo.Text);
                    deleteCommand.ExecuteNonQuery();
                    MessageBox.Show("Deleted..."); //Silme işlemini gerçekleştirdikten sonra kullanıcıya mesaj verdik.
                    dataFiiling();

                }
            }
            else
                MessageBox.Show("Not Found Stock.");
            connection.Close();
        }

        private void btnUpdateStock_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string model = txtModel.Text;
            string piece = txtPiece.Text;
            string serialNo = txtSerialNo.Text;
            string saveName = txtSave.Text;


            sqlCommand = new SqlCommand( "update Kayıt set adı=@pAdi, model=@pModel, adet=@pAdet, kayıtYapan=@pKayitYapan where seriNo="+ selectedRow , connection);

            connection.Open();
            sqlCommand.Parameters.Add("@pAdi", SqlDbType.Char, 50).Value = name;
            sqlCommand.Parameters.Add("@pModel", SqlDbType.Char, 50).Value = model;
            sqlCommand.Parameters.Add("@pAdet", SqlDbType.Char, 50).Value = piece;  
            sqlCommand.Parameters.Add("@pSerino", SqlDbType.Char, 50).Value = serialNo;
            sqlCommand.Parameters.Add("@pKayitYapan", SqlDbType.Char, 50).Value = saveName;


            sqlCommand.ExecuteNonQuery();
            connection.Close();

            UPDATE();
        }
        private void UPDATE()
        {
            sqlDataAdapter = new SqlDataAdapter("Select * from Kayıt", connection);
            dataSet = new DataSet();
            connection.Open();
            sqlDataAdapter.Fill(dataSet, "Kayıt");
            connection.Close();
            dataGridView1.DataSource = dataSet.Tables["Kayıt"];
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Close();

            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = Convert.ToInt32(dataSet.Tables["Kayıt"].Rows[e.RowIndex]["seriNo"]);


            txtSerialNo.Text = selectedRow.ToString();
            txtName.Text = dataSet.Tables["Kayıt"].Rows[e.RowIndex]["adı"].ToString();
            txtModel.Text = dataSet.Tables["Kayıt"].Rows[e.RowIndex]["model"].ToString();
            txtPiece.Text = dataSet.Tables["Kayıt"].Rows[e.RowIndex]["adet"].ToString();
            txtSerialNo.Text = dataSet.Tables["Kayıt"].Rows[e.RowIndex]["seriNo"].ToString();
            txtSave.Text = dataSet.Tables["Kayıt"].Rows[e.RowIndex]["kayıtYapan"].ToString();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            connection.Open();

            int srg = int.Parse(txtSearchSerialNo.Text);

            string query = "Select * from Kayıt where seriNo Like '" + srg + "'";

            SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);

            DataSet dataSet = new DataSet();

            dataAdapter.Fill(dataSet, "Kayıt");

            this.dataGridView1.DataSource = dataSet.Tables[0];

            connection.Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dataFiiling();
        }

        public void reportToPDF(DataGridView dataGridView,string filename)
        {
            BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.EMBEDDED);
            PdfPTable pdfPTable = new PdfPTable(dataGridView.Columns.Count);
            pdfPTable.DefaultCell.Padding = 3;
            pdfPTable.WidthPercentage = 100;
            pdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPTable.DefaultCell.BorderWidth = 1;

            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.NORMAL);

            foreach (DataGridViewColumn dataGridViewColumn in dataGridView.Columns)
            {
                PdfPCell pdfPCell = new PdfPCell(new Phrase(dataGridViewColumn.HeaderText,font));
                pdfPCell.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
                pdfPTable.AddCell(pdfPCell);
            }
            foreach (DataGridViewRow dataGridViewRow  in dataGridView.Rows)
            {
                foreach (DataGridViewCell dataGridViewCell in dataGridViewRow.Cells)
                {
                    pdfPTable.AddCell(new Phrase(dataGridViewCell.Value.ToString(), font));

                }
            }

            var savefiledialoge = new SaveFileDialog();
            savefiledialoge.FileName = filename;
            savefiledialoge.DefaultExt = ".pdf";
            if (savefiledialoge.ShowDialog() == DialogResult.OK) ;
            {
                using (FileStream fileStream = new FileStream(savefiledialoge.FileName,FileMode.Create))
                {
                    Document document = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    PdfWriter.GetInstance(document, fileStream);
                    document.Open();
                    document.Add(pdfPTable);
                    document.Close();
                    fileStream.Close();
                }
            }

        } 
        private void btnReport_Click(object sender, EventArgs e)
        {
            reportToPDF(dataGridView1,"Report");
        }
    }
}
