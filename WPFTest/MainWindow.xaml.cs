using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Media;

namespace WPFTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {     
        List<MatrixElement> _board;
     
        List<int> _marked;
        public string rows { get; set; }
        public string columns { get; set; }

        private string LEVELPATH = "lights-out-levels.json";

        private string ON_IMAGE= "IMAGE_on.png";

        private string OFF_IMAGE = "IMAGE_off.png";

        private int counter = 0;
        public MainWindow()
        {
            InitializeComponent();
            LoadLevelsContent();
        }

       
      
        private void CreateDrawings(int rows, int columns ,List<int> markedon)
        {       
            _board = new List<MatrixElement>();            
            int m = 0;
            for (int r = 0; r < rows; r++) {
                for (int c = 0; c < columns; c++) {
                    bool ismarked = markedon.Contains(m);
                    if (ismarked) { _board.Add(new MatrixElement(r, c) { Image = ON_IMAGE }); }
                    else{ _board.Add(new MatrixElement(r, c) { Image = OFF_IMAGE }); }                  
                    m++;      
                }                
            }
            Board.ItemsSource = _board;         
        }
         
        private void CellClick(object sender, MouseButtonEventArgs e)
        {    
            var border = (Border)sender;        
            var point = (MatrixElement)border.Tag;            
          
            this.counter++;
            User_moves.Text = this.counter.ToString();           
            RenderStage(point.X, point.Y);
            Debug.WriteLine("Clicked on " + point.X.ToString() + " " + point.Y.ToString() + " IMG " + point.Image.ToString());
        }

    public void LoadLevel(string level)
    {
        _marked = new List<int>();       
        using (StreamReader r = new StreamReader(LEVELPATH))
        {
            string json = r.ReadToEnd();
            List<Level> items = JsonConvert.DeserializeObject<List<Level>>(json);
                foreach (var item in items)
                {
                    if (item.name.ToString() == level )
                    {
                        Debug.WriteLine("Loaded "+item.name + " with c:" + item.columns + " and r:" + item.rows );
                        this.rows = item.rows.ToString();
                        this.columns = item.columns.ToString();
                        _marked.AddRange(item.on);                      
                    }               

                }
                CreateDrawings(Int32.Parse(rows), Int32.Parse(columns), _marked);
            }  
     }


        public void LoadLevelsContent()
        {
           
            using (StreamReader r = new StreamReader(LEVELPATH))
            {
                string json = r.ReadToEnd();
                List<Level> items = JsonConvert.DeserializeObject<List<Level>>(json);
                foreach (var item in items)
                {                    
                    cmbGamelevel.Items.Add(item.name);
                }
                cmbGamelevel.SelectedIndex = 0;
            }
        }

        public void RenderStage(int x, int y)
        {
            List<MatrixElement> next_step_matrix = new List<MatrixElement>();            

            var currentpoint = _board.FirstOrDefault(r => r.X == x && r.Y == y);
            if (currentpoint != null) {   if (currentpoint.Image == OFF_IMAGE) { currentpoint.Image = ON_IMAGE;   } else if (currentpoint.Image == ON_IMAGE) { currentpoint.Image = OFF_IMAGE; }  }

            var wpoint = _board.FirstOrDefault(r => r.X == (x-1) && r.Y == y);
            if (wpoint != null) { if (wpoint.Image == OFF_IMAGE) { wpoint.Image = ON_IMAGE; } else if (wpoint.Image == ON_IMAGE) { wpoint.Image = OFF_IMAGE; } }

            var npoint = _board.FirstOrDefault(r => r.X == x && r.Y == (y-1));
            if (npoint != null) { if (npoint.Image == OFF_IMAGE) { npoint.Image = ON_IMAGE; } else if (npoint.Image == ON_IMAGE) { npoint.Image = OFF_IMAGE; } }

            var epoint = _board.FirstOrDefault(r => r.X == (x+1) && r.Y == y);
            if (epoint != null) { if (epoint.Image == OFF_IMAGE) { epoint.Image = ON_IMAGE; } else if (epoint.Image == ON_IMAGE) { epoint.Image = OFF_IMAGE; } }

            var spoint = _board.FirstOrDefault(r => r.X == x && r.Y == (y+1));
            if (spoint != null) { if (spoint.Image == OFF_IMAGE) { spoint.Image = ON_IMAGE; } else if (spoint.Image == ON_IMAGE) { spoint.Image = OFF_IMAGE; } }


            foreach (var el in _board)
            {
                next_step_matrix.Add(new MatrixElement(el.X, el.Y) { Image = el.Image });
            }            

            Board.ItemsSource = next_step_matrix;
            var onpoints = next_step_matrix.FirstOrDefault(r => r.Image==ON_IMAGE);
            if (onpoints == null) { 
                txtGameStatus.Visibility = Visibility.Visible; 
                Board.Background = Brushes.Green;  
            }
            else if (onpoints != null) { 
                txtGameStatus.Visibility = Visibility.Collapsed; 
                Board.Background = Brushes.Red; 
            }
        }
    

    private void Button_Click(object sender, RoutedEventArgs e)
    {
            this.counter = 0;
            User_moves.Text = this.counter.ToString();
            txtGameStatus.Visibility = Visibility.Collapsed;
            Board.Background = Brushes.Red;
            string levelname = cmbGamelevel.SelectedItem.ToString();
            LoadLevel(levelname);                  
    }
   public class Level 
        {
            public string name { get; set; }
            public int columns { get; set; }
            public int rows { get; set; }
            public List<int> on { get; set; }
        }

    }
}
