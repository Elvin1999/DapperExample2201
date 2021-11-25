using Dapper;
using DapperExample2.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
using static System.Configuration.ConfigurationManager;
namespace DapperExample2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Player> GetAll()
        {
            List<Player> players = new List<Player>();
            using (var connection = new SqlConnection(ConnectionStrings["MyConnString"].ConnectionString))
            {
                players = connection.Query<Player>("SELECT Id,Name,Score,IsStar from Players").ToList();
            }
            return players;
        }


        public Player GetById(int id)
        {
            using (var connection=new SqlConnection(ConnectionStrings["MyConnString"].ConnectionString))
            {
                var player = connection.QueryFirstOrDefault("select * from Players where Id=@PId",
                    new { PId = id });

                return new Player
                {
                    Id = player.Id,
                    Name = player.Name,
                    Score = player.Score,
                    IsStar = player.IsStar
                };

            }
        }
        public void Update(Player player)
        {
            using (var connection = new SqlConnection(ConnectionStrings["MyConnString"].ConnectionString))
            {
                connection.Execute("Update Players SET Name=@PName,Score=@PScore,IsStar=@PIsStar where Id=@PId",
                    new { PId = player.Id, PName = player.Name, PScore = player.Score, PIsStar = player.IsStar });

            }
        }


        public void Insert(Player player)
        {
            using (var connection=new SqlConnection(ConnectionStrings["MyConnString"].ConnectionString))
            {
                connection.Execute(@"INSERT INTO Players(Name,Score,IsStar)
                                        VALUES(@PName,@PScore,@PIsStar)",
                                     new { @PName = player.Name, @PScore = player.Score, @PIsStar = player.IsStar });
            }
            MessageBox.Show("Player added successfully");  
        }

        public void Delete(int id)
        {
            using (var connection=new SqlConnection(ConnectionStrings["MyConnString"].ConnectionString))
            {
                connection.Execute("DELETE FROM Players WHERE Id=@PId", new { PId = id });
                MessageBox.Show("Delete Successfully");
            }
        }

        public void CallSp()
        {
            using (var connection=new SqlConnection(ConnectionStrings["MyConnString"].ConnectionString))
            {
                   DynamicParameters parameters= new DynamicParameters();
                parameters.Add("@PScore",80, DbType.Double);
                var data = connection.Query<Player>("ShowGreaterThan",parameters, commandType:CommandType.StoredProcedure);
                mydatagrid.ItemsSource = data.ToList();
            }
        }
        public MainWindow()
        {
            InitializeComponent();

            //var player = GetById(1);
            //player.Name = "Rafiq Mammadli";
            //player.Score = 0;
            //Update(player);

            //Insert(new Player
            //{
            //    Name = "Omer",
            //    IsStar = false,
            //    Score = 77.65
            //});
            //Delete(1);
            // mydatagrid.ItemsSource = GetAll();
            CallSp();

        }
    }
}
