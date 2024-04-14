using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace eventos
{
    internal class Program
    {
        private static List<Form1> ventanasClonadas = new List<Form1>();
        private static int cantidadBotonesAgregados = 0;
        private static int eliminarCantVentanas = 0;
        private static int contarVentanas = 0;
        private static Timer timer = new Timer();

        static void Main(string[] args)
        {
            Form form = new Form();
            Button botoncito = new Button();
            Label labelCentro = new Label(); // Nuevo Label para el centro
            //labelCentro.Text = "Clonar ventanas";
            //labelCentro.Location = new Point(0, 50);
            botoncito.Text = "HOLA!";
            form.Width = 300;
            form.Height = 150;
            botoncito.Width = 50;
            botoncito.Height = 30;
            form.BackColor = Color.Pink;
            botoncito.BackColor = Color.White;
            botoncito.Font = new Font("Arial", 8, FontStyle.Bold);
            InicializarBoton(botoncito, form, timer);
            timer.Interval = 20;
            timer.Start();
            //form.Controls.Add(labelCentro);
            form.Controls.Add(botoncito);
            Application.Run(form);
        }
        private static void InicializarBoton(Button botoncito, Form form, Timer timer)
        {
            botoncito.Location = new Point(25, 25);
            int velocidadX = 3;
            int velocidadY = 3;
            botoncito.Click += (sender, e) =>
            {

                form.Close();
            };

            form.KeyPreview = true;
            form.KeyDown += (sender, e) =>
            {
                // Muestra un mensaje al presionar una tecla
                MessageBox.Show($"Logrado");
            };

            form.Click += (sender, e) =>
            {
                Application.Restart();
            };
            timer.Tick += (sender, e) =>
            {
                MoverBotones(form, botoncito, ref velocidadX, ref velocidadY);
            };
        }
        private static void MoverBotones(Form form, Button botoncito, ref int velocidadX, ref int velocidadY)
        {

            int actualX = botoncito.Location.X + velocidadX;
            int actualY = botoncito.Location.Y + velocidadY;

            if (actualX <= 0 || actualX >= form.Width - botoncito.Width)
            {
                velocidadX = -velocidadX;
                actualX = Math.Max(0, Math.Min(form.Width - botoncito.Width, actualX));

                if (contarVentanas < 10 && actualX <= 0 && !ventanasClonadas.Exists(v => v.Visible))
                {
                    // Crea un clon idéntico de la ventana y lo muestra
                    Form1 nuevaVentana = ClonarVentana(form, botoncito);
                    nuevaVentana.Show();
                    ventanasClonadas.Add(nuevaVentana);
                    contarVentanas++;
                }

                if (actualX <= (form.Width - botoncito.Width))
                {
                    // Agrega un nuevo botón con las mismas propiedades
                    AgregarNuevoBoton(form);
                   
                }
            }
            if (actualY <= 0 || actualY >= form.Height - botoncito.Height)
            {
                velocidadY = -velocidadY;

                actualY = Math.Max(0, Math.Min(form.Height - botoncito.Height, actualY));

            }
            if (actualY >= form.Height - botoncito.Height && cantidadBotonesAgregados > 0)
            {
              
                EliminarUltimoBoton(form);
            }
        
            if (actualY <= 0 && ventanasClonadas.Count > 0)
            {
                EliminarVentanas(form);
            }
            VerificarEsquinas(form, botoncito, actualX, actualY);
            botoncito.Location = new Point(actualX, actualY);
        }
        private static void EliminarVentanas(Form form)
        {
            if (ventanasClonadas.Count > 0)
            {
                Form1 ventanaAEliminar = ventanasClonadas[0];
                ventanasClonadas.RemoveAt(0);
                ventanaAEliminar.Close();
                eliminarCantVentanas--;
            }
        }

        private static void VerificarEsquinas(Form form, Button botoncito, int actualX, int actualY)
        {
            // Resto del código para manejar las esquinas
            if (actualX >= (form.Width - botoncito.Width) && actualY <= botoncito.Height)
            {
                form.Width += 20;
                form.Height += 20;
                form.BackColor = Color.Cyan;
            }
            else if (actualX >= (form.Width - botoncito.Width) && actualY >= (botoncito.Height + 50))
            {
                if (botoncito.Width > 0 && botoncito.Height > 0)
                {
                    botoncito.Width -= 5;
                    botoncito.Height -= 5;
                    botoncito.BackColor = Color.BlueViolet;
                }
                else
                {
                    MessageBox.Show("El botoncito desapareció");
                    Application.Exit();
                }
            }
            else if (actualX <= botoncito.Width && actualY >= (form.Height - (botoncito.Height)))
            {
                form.Width -= 1;
                form.Height -= 1;
                form.BackColor = Color.LightGreen;
            }
            else if (actualX <= (botoncito.Width - 25) && actualY <= (botoncito.Height - 25))
            {
                botoncito.Width += 1;
                botoncito.Height += 1;
                botoncito.BackColor = Color.HotPink;
            }
        }


        private static Form1 ClonarVentana(Form form, Button botonOriginal)
        {
            Form1 nuevaVentana = new Form1();
            nuevaVentana.BackColor = form.BackColor;
            nuevaVentana.Width = form.Width;
            nuevaVentana.Height = form.Height;

            // Clona el botón original y aplica las mismas propiedades y eventos
            Button nuevoBoton = new Button();
            nuevoBoton.Text = botonOriginal.Text;
            nuevoBoton.BackColor = botonOriginal.BackColor;
            nuevoBoton.ForeColor = botonOriginal.ForeColor;
            nuevoBoton.Width = botonOriginal.Width;
            nuevoBoton.Height = botonOriginal.Height;
            nuevoBoton.Font = botonOriginal.Font;

        
            nuevoBoton.Location = new Point(25, 25);

            nuevoBoton.Click += (sender, e) =>
            {
               
                nuevaVentana.Close();
            };

            // Agrega el nuevo botón clonado al formulario clonado
            nuevaVentana.Controls.Add(nuevoBoton);

            // Inicializa los eventos y la lógica del nuevo botón clonado
            InicializarBoton(nuevoBoton, nuevaVentana, timer);

            return nuevaVentana;
        }

        private static void AgregarNuevoBoton(Form form)
        {
          
            Button nuevoBoton = new Button();
            nuevoBoton.Text = "HOLA!";
            nuevoBoton.BackColor = Color.White;
            nuevoBoton.ForeColor = Color.Black;
            nuevoBoton.Width = 50;
            nuevoBoton.Height = 30;
            nuevoBoton.Font = new Font("Arial", 8, FontStyle.Bold);
            nuevoBoton.Location = new Point(15, 15);
            int velocidadX = 3;
            int velocidadY = 3;
            // EVENTO AL HACER CLIC EN EL BOTÓN AGREGADO
            nuevoBoton.Click += (sender, e) =>
            {
                // Cierra el programa al hacer clic en el botón agregado
                form.Close();
            };
          
            if (cantidadBotonesAgregados < 10)
            {
                form.Controls.Add(nuevoBoton);
                cantidadBotonesAgregados++;
            }
            timer.Tick += (sender, e) =>
            {

                int actualX = nuevoBoton.Location.X + velocidadX;
                int actualY = nuevoBoton.Location.Y + velocidadY;

                if (actualX <= 0 || actualX >= form.Width - nuevoBoton.Width)
                {
                    velocidadX = -velocidadX;
                    actualX = Math.Max(0, Math.Min(form.Width - nuevoBoton.Width, actualX));
                }
                if (actualY <= 0 || actualY >= form.Height - nuevoBoton.Height)
                {
                    velocidadY = -velocidadY;

                    actualY = Math.Max(0, Math.Min(form.Height - nuevoBoton.Height, actualY));

                }
                if (actualY >= form.Height - nuevoBoton.Height && cantidadBotonesAgregados > 0)
                {
                   
                    EliminarUltimoBoton(form);
                }
                nuevoBoton.Location = new Point(actualX, actualY);
            };
        }
        private static void EliminarUltimoBoton(Form form)
        {
            if (form.Controls.Count > 1)
            {
                Control ultimoControl = form.Controls[form.Controls.Count - 1];
                form.Controls.Remove(ultimoControl);
                cantidadBotonesAgregados--;
            }
        }

    }
}