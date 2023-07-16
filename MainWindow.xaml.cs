using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace ExamenQuintanaAllan
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            this.txtRut.Clear();
            this.txtDigiRut.Clear();
            this.txtNombre.Clear();
            this.txtDia.Clear();
            this.txtMes.Clear();
            this.txtAno.Clear();
            this.rdSacarNuevaLicencia.IsChecked = false;
            this.rdRenovarLicencia.IsChecked = false;
            this.rdA1.IsChecked = false;
            this.rdA2.IsChecked = false;
            this.rdB.IsChecked = false;
            this.rdC.IsChecked = false;
            this.txtTeorico.Clear();
            this.txtPractico.Clear();
            this.txtMedico.Clear();
            this.txtValidar.Clear();
            this.txtLicencia.Clear();
        }

        private void btnValidar_Click(object sender, RoutedEventArgs e)
        {
            string[] datos = new string[12];

            datos[0] = this.txtRut.Text;
            datos[1] = this.txtDigiRut.Text;
            datos[2] = this.txtNombre.Text;
            datos[3] = this.txtDia.Text;
            datos[4] = this.txtMes.Text;
            datos[5] = this.txtAno.Text;

            if (this.rdSacarNuevaLicencia.IsChecked == true)
                datos[6] = "Sacar nueva licencia";
            else if (this.rdRenovarLicencia.IsChecked == true)
                datos[6] = "Renovar licencia";
            else
                datos[6] = "";

            if (this.rdA1.IsChecked == true)
                datos[7] = "A1";
            else if (this.rdA2.IsChecked == true)
                datos[7] = "A2";
            else if (this.rdB.IsChecked == true)
                datos[7] = "B";
            else if (this.rdC.IsChecked == true)
                datos[7] = "C";
            else
                datos[7] = "";

            datos[8] = this.txtTeorico.Text;
            datos[9] = this.txtPractico.Text;
            datos[10] = this.txtMedico.Text;

            if (ValidarDatos(datos))
            {
                string rut = datos[0];
                string digitoRut = datos[1];
                string nombre = datos[2];
                int dia = int.Parse(datos[3]);
                int mes = int.Parse(datos[4]);
                int ano = int.Parse(datos[5]);
                string tipoLicencia = datos[7];
                string categoriaLicencia = datos[7];
                string medico = datos[8];
                string validar = datos[9];
                string licencia = datos[10];

                int puntajeTeorico = int.Parse(datos[8]);
                int puntajePractico = int.Parse(datos[9]);
                int puntajeMedico = int.Parse(datos[10]);

                int costoExamenTeorico = 6000;
                int costoExamenPractico = 8000;
                int costoExamenMedico = 10000;

                int totalPuntos = puntajeTeorico + puntajePractico + puntajeMedico;
                int totalPagar = 0;

                string mensajeLicencia = "";

                if (tipoLicencia == "A1" || tipoLicencia == "A2")
                {
                    if (ano <= DateTime.Now.Year - 20)
                    {
                        if (puntajePractico >= 500 && totalPuntos > 800)
                        {
                            mensajeLicencia = $"Obtuvo la licencia clase {tipoLicencia}.";
                            totalPagar = costoExamenTeorico + costoExamenPractico + costoExamenMedico;
                        }
                        else
                        {
                            txtValidar.Text = "No cumple los requisitos para obtener la licencia.";
                            return;
                        }
                    }
                    else
                    {
                        txtValidar.Text = "Debe tener 20 años o más para obtener la licencia.";
                        return;
                    }
                }
                else if (tipoLicencia == "B" || tipoLicencia == "C")
                {
                    if (ano <= DateTime.Now.Year - 18)
                    {
                        if (puntajeMedico >= 300 && totalPuntos > 600)
                        {
                            mensajeLicencia = $"Obtuvo la licencia clase {tipoLicencia}.";
                            totalPagar = costoExamenTeorico + costoExamenPractico + costoExamenMedico;
                        }
                        else
                        {
                            txtValidar.Text = "No cumple los requisitos para obtener la licencia.";
                            return;
                        }
                    }
                    else
                    {
                        txtValidar.Text = "Debe tener 18 años o más para obtener la licencia.";
                        return;
                    }
                }
                else
                {
                    txtValidar.Text = "Debe seleccionar un tipo de licencia.";
                    return;
                }

                if (datos[6] == "Renovar licencia")
                {
                    totalPagar -= (int)(totalPagar * 0.1);
                }

                txtValidar.Text = $"{mensajeLicencia} Debe pagar un total de ${totalPagar}.";
            }
        }

        private bool ValidarDatos(string[] datos)
        {
            int[] indicesNumericos = { 0, 1, 3, 4, 5, 8, 9, 10 };

            foreach (int indice in indicesNumericos)
            {
                if (string.IsNullOrWhiteSpace(datos[indice]))
                {
                    txtValidar.Text = "No debes dejar valores vacíos.";
                    return false;
                }

                if (!EsEntero(datos[indice], out int valorInvalido))
                {
                    txtValidar.Text = $"El valor '{datos[indice]}' no es un dato válido.";
                    return false;
                }
            }

            int ano = int.Parse(datos[5]);
            if (ano < 1900)
            {
                txtValidar.Text = "El año de nacimiento debe ser igual o posterior a 1900.";
                return false;
            }

            string tipoLicencia = datos[6];
            string categoriaLicencia = datos[7];

            if (tipoLicencia == "" || categoriaLicencia == "")
            {
                txtValidar.Text = "Debes seleccionar un tipo y una categoría de licencia.";
                return false;
            }

            string rut = datos[0];
            string digitoRut = datos[1];

            if (digitoRut.Length != 1 || !char.IsDigit(digitoRut[0]))
            {
                txtValidar.Text = "El dígito verificador del Rut debe ser un único dígito numérico.";
                return false;
            }

            txtValidar.Text = string.Empty;
            return true;
        }

        private bool EsEntero(string valor, out int resultado)
        {
            if (int.TryParse(valor, out resultado))
            {
                return true;
            }

            return false;
        }

        private void btnLicencia_Click(object sender, RoutedEventArgs e)
        {
            string rut = txtRut.Text;
            string digitoRut = txtDigiRut.Text;
            string nombre = txtNombre.Text;
            string tipoLicencia = "";
            string fechaVencimiento = "";

            if (rdA1.IsChecked == true)
                tipoLicencia = "A1";
            else if (rdA2.IsChecked == true)
                tipoLicencia = "A2";
            else if (rdB.IsChecked == true)
                tipoLicencia = "B";
            else if (rdC.IsChecked == true)
                tipoLicencia = "C";

            int anoNacimiento = int.Parse(txtAno.Text);
            int anoVencimiento = DateTime.Now.Year + 6;
            fechaVencimiento = $"{txtDia.Text}-{txtMes.Text}-{anoVencimiento}";

            string datosLicencia = $"{rut}-{digitoRut};{nombre};{tipoLicencia};{fechaVencimiento}";

            string archivoLicencias = @"C:\Users\allan\Documents\Programas\ExamenQuintanaAllan\bin\Debug\licenciaconducir.txt";
            using (StreamWriter sw = File.AppendText(archivoLicencias))
            {
                sw.WriteLine(datosLicencia);
            }

            txtLicencia.Text = $"Licencia:\n\n{datosLicencia}";
        }
    }
}
