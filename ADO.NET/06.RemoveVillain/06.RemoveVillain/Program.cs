using Microsoft.Data.SqlClient;
using System;

namespace _06.RemoveVillain
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection connection = new SqlConnection(@"Server=.\SQLEXPRESS; Database = MinionsDB; Integrated Security=true");
            connection.Open();

            string inputId = Console.ReadLine();

            using (connection)
            {
                SqlCommand checkIfVillainExists = new SqlCommand(@"SELECT * FROM Villains WHERE Id = @villainId", connection);
                checkIfVillainExists.Parameters.AddWithValue("@villainId", inputId);

                int? villainId = (int?)checkIfVillainExists.ExecuteScalar();

                if (villainId == null)
                {
                    Console.WriteLine("No such villain was found.");
                    return;
                }

                SqlCommand getVillinName = new SqlCommand($@"SELECT Name FROM Villains WHERE Id = '{inputId}'", connection);

                string villainName = (string)getVillinName.ExecuteScalar();

                SqlCommand deleteVillainMinions = new SqlCommand($@"DELETE FROM MinionsVillains WHERE VillainId = '{inputId}'", connection);

                int countMinions = deleteVillainMinions.ExecuteNonQuery();

                SqlCommand deleteVillain = new SqlCommand($@"DELETE FROM Villains WHERE Id = '{inputId}'", connection);

                deleteVillain.ExecuteNonQuery();

                Console.WriteLine($"{villainName} was deleted.");
                Console.WriteLine($"{countMinions} minions were released.");
            }
        }
    }
}
