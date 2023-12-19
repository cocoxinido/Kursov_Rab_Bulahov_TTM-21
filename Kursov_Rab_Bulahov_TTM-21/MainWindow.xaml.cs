using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
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

namespace Kursov_Rab_Bulahov_TTM_21
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Movie> pictures = new List<Movie>();
        List<Movie> TempPictures = new List<Movie>();

        static string NameFileJSON = "PicturesCatalog.json";
        public MainWindow()
        {
            InitializeComponent();

            Categories.Items.Add("All");

            if (File.Exists(NameFileJSON))
            {
                List<Movie> readedPicturs = JsonSerializer.Deserialize<List<Movie>>(File.ReadAllText(NameFileJSON));

                foreach (Movie pic in readedPicturs)
                {
                    pictures.Add(pic);

                    List_Movie.Items.Add(pic.Name);

                    if (!(Categories.Items.Contains(pic.Categ)))
                        Categories.Items.Add(pic.Categ);

                }
            }
        }

        private void Categories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Categories.SelectedIndex != -1)
            {
                List_Movie.Items.Clear();
                if (Categories.SelectedItem.ToString().Equals("All"))
                {
                    foreach (Movie pic in pictures)
                        List_Movie.Items.Add(pic.Name);
                    return;
                }

                TempPictures.Clear();

                foreach (Movie pic in pictures)
                {
                    if (pic.Categ == Categories.SelectedItem.ToString())
                    {
                        List_Movie.Items.Add(pic.Name);
                        TempPictures.Add(pic);
                    }
                }
            }
        }
        static ImageSource ByteToImage(byte[] imageData)
        {
            var bitmap = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            bitmap.BeginInit();
            bitmap.StreamSource = ms;
            bitmap.EndInit();

            return (ImageSource)bitmap;
        }

        private void List_Movie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((Categories.SelectedIndex == 0) && (Str_Search.Text.Length == 0 && Str_Search_Tag.Text.Length == 0))
            {
                if (List_Movie.Items.Count > 0 && List_Movie.SelectedIndex != -1)
                {
                    Pic_Movie.Source = ByteToImage(Convert.FromBase64String(pictures[List_Movie.SelectedIndex].Img));
                }
            }
            else
            {
                if (TempPictures.Count > 0 && List_Movie.SelectedIndex != -1)
                {
                    Pic_Movie.Source = ByteToImage(Convert.FromBase64String(TempPictures[List_Movie.SelectedIndex].Img));
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string jsonString = JsonSerializer.Serialize(pictures);
            File.WriteAllText(NameFileJSON, jsonString);
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            if (List_Movie.SelectedIndex != -1)
            {
                pictures.Remove(pictures[List_Movie.SelectedIndex]);
                List_Movie.Items.Clear();

                foreach (Movie pic in pictures)
                    List_Movie.Items.Add(pic.Name);

                Categories.Items.Clear();
                Categories.Items.Add("All");

                foreach (Movie pic in pictures)
                {
                    if (!(Categories.Items.Contains(pic.Categ)))
                        Categories.Items.Add(pic.Categ);
                }
                Categories.SelectedIndex = 0;
            }
        }

        private void Load_File_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (!(bool)dlg.ShowDialog())
                return;

            Uri fileUri = new Uri(dlg.FileName);

            Add_File addPic = new Add_File();

            if (addPic.ShowDialog() == true)
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(dlg.FileName);
                string base64ImageRepresentation = Convert.ToBase64String(imageArray);

                Movie pic = new Movie(addPic.Name1.Text, base64ImageRepresentation, addPic.Categorie.Text);

                pictures.Add(pic);
                List_Movie.Items.Add(pic.Name);

                if (!(Categories.Items.Contains(pic.Categ)))
                    Categories.Items.Add(pic.Categ);
            }
        }

        private void Load_URL_Click(object sender, RoutedEventArgs e)
        {
            Add_URL addPic = new Add_URL();

            if (addPic.ShowDialog() == true)
            {
                WebClient client = new WebClient();
                string imageUrl = addPic.Str_URL.Text;
                byte[] imageArray = client.DownloadData(imageUrl);
                string base64ImageRepresentation = Convert.ToBase64String(imageArray);

                Movie pic = new Movie(addPic.Name_str.Text, base64ImageRepresentation, addPic.Categorie_str.Text);

                pictures.Add(pic);
                List_Movie.Items.Add(pic.Name);

                if (!(Categories.Items.Contains(pic.Categ)))
                    Categories.Items.Add(pic.Categ);
            }
        }

        private void Add_Tag_Click(object sender, RoutedEventArgs e)
        {
            if (List_Movie.SelectedIndex != -1 && List_Movie.Items.Count == pictures.Count && Str_Add_Tag.Text.Length > 0)
            {
                pictures[List_Movie.SelectedIndex].add_tag(Str_Add_Tag.Text);
            }
            else
                MessageBox.Show("Select category all or fill in the tag field");
        }

        private void Search_Tag_Click(object sender, RoutedEventArgs e)
        {
            List_Movie.Items.Clear();
            TempPictures.Clear();
            foreach (Movie pic in pictures)
            {
                foreach (string tag in pic.Tags)
                {
                    if (tag.ToLower().Equals(Str_Search_Tag.Text.ToLower()))
                    {
                        List_Movie.Items.Add(pic.Name);
                        TempPictures.Add(pic);
                    }
                }
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            List_Movie.Items.Clear();
            TempPictures.Clear();
            foreach (Movie pic in pictures)
            {
                if (pic.Name.ToLower().Contains(Str_Search.Text.ToLower()))
                {
                    List_Movie.Items.Add(pic.Name);
                    TempPictures.Add(pic);
                }
            }
        }
    }
}