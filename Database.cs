using System;
using System.IO;
using SQLite;
using System.Collections.Generic;

namespace Hackathon
{
    [Table("Items")]
    public class Database
    {
        public Database()
        {
            Stored_Viberate = settings.Viberate;
            Stored_HighScore = settings.HighScore;
            Stored_SelectedMonth = settings.SelectedMonth;
            Stored_Help = settings.Help;
        }

        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }


        //MUST BE PROPERTIES
        public bool Stored_Viberate { get; set; }
        public int Stored_SelectedMonth { get; set; }
        public int Stored_HighScore { get; set; }
        public bool Stored_Help { get; set; }

}

    public static class DatabaseManagement
    {
        private static void Add()
        {
            string output = "";
            output += "\nCreating database, if it doesn't already exist";
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdata.db3");

            SQLiteConnection db = new SQLiteConnection(dbPath);
            db.CreateTable<Database>();

            Database newData = new Database();
            newData.Stored_Viberate = settings.Viberate;
            newData.Stored_HighScore = settings.HighScore;
            newData.Stored_SelectedMonth = settings.SelectedMonth;
            newData.Stored_Help = settings.Help;

            Console.WriteLine("Viberate: {0}  High Score: {1}  Selected Month: {2}", settings.Viberate, settings.HighScore, settings.SelectedMonth);

            db.Insert(newData);

            Console.WriteLine(output);
        }
        public static void SetSettings()  //Access database and set settings values from accessed data
        {
            string output = "";
            output += "\nGet query example: ";
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdata.db3");

            SQLiteConnection db = new SQLiteConnection(dbPath);

            TableQuery<Database> table = db.Table<Database>();
            try
            {
                if (table.Count() > 0)
                {
                    foreach(var s in table)
                    {
                    Console.WriteLine("Viberate: {0}  High Score: {1}  Selected Month: {2}", s.Stored_Viberate, s.Stored_HighScore, s.Stored_SelectedMonth);
                    settings.SelectedMonth = s.Stored_SelectedMonth;
                    settings.Viberate = s.Stored_Viberate;
                    settings.HighScore = s.Stored_HighScore;
                    settings.Help = s.Stored_Help;
                    Console.WriteLine("Accessing entry");
                    }
                }
                else
                {
                    Console.WriteLine("Add new entry");
                    settings.SelectedMonth = 0;
                    settings.Viberate = true;
                    settings.HighScore = 0;
                    settings.Help = true;
                    Add();
                }
            }
            catch
            {
                Console.WriteLine("catch add new entry");
                settings.SelectedMonth = 0;
                settings.Viberate = true;
                settings.HighScore = 0;
                settings.Help = true;
                Add();
            }

        }
        public static void UpdateData() //Delete listing from database, add new one
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdata.db3");

            SQLiteConnection db = new SQLiteConnection(dbPath);

            TableQuery<Database> table = db.Table<Database>();
            try
            {
                if (table != null && table.Count() > 0)
                {
                    foreach (var s in table)
                    {
                        Console.WriteLine("Deleting item");
                        Delete(s.Id);
                    }
                }
            }
            catch { Console.WriteLine("Empty Database"); }

            Add();
        }
        private static string Delete(int id)
        {
            string output = "";
            output += "\nDelete query example: ";
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "ormdata.db3");

            SQLiteConnection db = new SQLiteConnection(dbPath);

            var rowcount = db.Delete(new Database() { Id = id });

            return output;
        }
    }
}