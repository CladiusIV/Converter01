using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Converter1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            // Properties
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Filter = "Audio files *.wav|*.wav";
            openFileDialog1.Title = "Browse wav files";
            //openFileDialog1.DefaultExt = "*.wav";
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.CheckFileExists = true;

        }
        
        private void button1_Click(object sender, EventArgs e) //Single file button
            
        {

            openFileDialog1.ShowDialog(); // Open the browser, must be after the properties.
            txtOrigen.Text = openFileDialog1.FileName; //Write the selected file into a combobox.

            string fileNameOrigen = txtOrigen.Text;
            string result;

            result = Path.GetFileNameWithoutExtension(fileNameOrigen);
            txtDestino.Text = result + ".mp3";
        }

        private void button3_Click(object sender, EventArgs e) // Multiple files button
        {

            openFileDialog1.Multiselect = true; // List of files
            openFileDialog1.ShowDialog(); // Open the browser, must be after the properties.

            foreach (string file in openFileDialog1.FileNames)
            {


                listBox1.Text = file; //Write the selected file into a combobox.

                string filesToList = listBox1.Text;
                string result;

                result = Path.GetFileNameWithoutExtension(filesToList) + ".mp3";
                listBox1.Text = result;
                // Agregar comprobacion si el item existe dar error

                //listBox1.Items.Add(result);
                
            }
        }

        private void button2_Click(object sender, EventArgs e) //Convert button
        {
            string error = "\n";
            bool esError = false;
            int presetSelect;
            //const string fileNameToExec = "lame.exe";
            presetSelect = cboPresets.SelectedIndex + 1;

            ProcessStartInfo start = new ProcessStartInfo(); //Constructor, inicializo el proceso.
            try
            {
                int i = (1 / txtOrigen.Text.Length); //Verifico que el campo no este vacio
                string inputName; //State var
                string outputName;

                inputName = txtOrigen.Text; //Get the info from the textbox into a var, and convert var to text
                outputName = txtDestino.Text;
                
                switch (presetSelect) //Select a preset from the list.
                {
                    case 1:
                        {
                            string presetSelect1;
                            presetSelect1 = "";
                            start.Arguments = string.Format("{0} {1} {2}", presetSelect1, inputName, outputName);
                            break;
                        }
                    case 2:
                        {
                            string presetSelect2;
                            presetSelect2 = "--preset cbr 192 ";
                            start.Arguments = string.Format("{0} {1} {2}", presetSelect2, inputName, outputName);
                            break;
                        }
                    case 3:
                        {
                            string presetSelect3;
                            presetSelect3 = "--preset cbr 256 ";
                            start.Arguments = string.Format("{0} {1} {2}", presetSelect3, inputName, outputName);
                            break;
                        }
                    case 4:
                        {
                            string presetSelect4;
                            presetSelect4 = "--preset insane ";
                            start.Arguments = string.Format("{0} {1} {2}", presetSelect4, inputName, outputName);
                            break;
                        }
                    default:
                        {
                            esError = true;
                            error += "Seleccione la calidad.";
                            break;
                        }
                }

                start.FileName = Directory.GetCurrentDirectory() + @"\Data\" + "lame.exe";
                // Console Window
                start.WindowStyle = ProcessWindowStyle.Normal;
                start.CreateNoWindow = true;
            }

            catch (DivideByZeroException)
            {
                esError = true;
                error = "Ingrese un archivo a convertir.";
            }

            int exitCode;

            //Check file exists.
            string checkFile;
            string checkDir;
            checkFile = txtDestino.Text;
            checkDir = Directory.GetCurrentDirectory();

            if (cboPresets.SelectedItem == null)
            {
            	// If the combobox is empty, throw the defaul value which is an error (Not selected).
            }

            else if (File.Exists(checkDir + @"\" + checkFile)) //If the file exist, error.
            {
                DialogResult dialogResult1 = MessageBox.Show("Sobreescribir el archivo destino?" + "\n" + checkDir + @"\" + checkFile, "El archivo ya existe!", MessageBoxButtons.OKCancel);

                if (dialogResult1 == DialogResult.OK)
                {
                    // Continues with the program and overwrite the file.
                }

                else if (dialogResult1 == DialogResult.Cancel)
                {
                    esError = true;
                    error += "Cancelado por el usuario.";
                }
            }

            else
            {
                //If not, continue with the program.
            }

            Debug.WriteLine(checkDir + @"\" + checkFile);

            try
            {
                int i = (1 / Convert.ToInt32(!esError)); // Check for error.

                /* Block of working code #1, bool.
                if (File.Exists(@"\Data\lame.exe"))
                {
                    //Sigue con el proceso normal.
                }

                else
                {
                    esError = true;
                    error += "El archivo codificador no esta dentro del directorio Data.";
                }
                */

                try // Block of working code 2#, handle exception.
                {
                    if (!File.Exists(@"\Data\"))
                    {
                        throw new FileNotFoundException();
                    }
                }
                catch (FileNotFoundException)
                {
                    esError = true;
                    error += "El archivo codificador no existe en el directorio Data.";
                }

                using (Process proc = Process.Start(start)) //Run and wait for it to finish.
                {
                    proc.WaitForExit();
                    exitCode = proc.ExitCode;
                }
            }

            catch
            {
                MessageBox.Show(error,"Error!");
            }
        }

        private void button4_Click(object sender, EventArgs e) // Carga varios archivos button
        {
            button1.Visible = false;
            button3.Visible = true;
            button4.Visible = false;
            button5.Visible = true;
            txtDestino.Visible = false;
            listBox1.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e) // Cancel button
        {
            button1.Visible = true;
            button3.Visible = false;
            button4.Visible = true;
            button5.Visible = false;
            txtDestino.Visible = true;
            listBox1.Visible = false;
            cboPresets.SelectedIndex = -1;

        }
    }
}
