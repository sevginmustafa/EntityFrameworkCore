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
<<<<<<< HEAD
                SqlCommand checkIfVillainExists = new SqlCommand(@"SELECT Name FROM Villains WHERE Id = @villainId", connection);
=======
                SqlCommand checkIfVillainExists = new SqlCommand(@"SELECT Id FROM Villains WHERE Id = @villainId", connection);
>>>>>>> b5a5852dab3067d35185a8c19bba6ee94e877a3e
                checkIfVillainExists.Parameters.AddWithValue("@villainId", inputId);

                string villainName = (string)checkIfVillainExists.ExecuteScalar();

                if (villainName == null)
                {
                    Console.WriteLine("No such villain was found.");
                    return;
                }

                SqlCommand deleteVillainMinions = new SqlCommand(@"DELETE FROM MinionsVillains WHERE VillainId = @villainId", connection);
                deleteVillainMinions.Parameters.AddWithValue("@villainId", inputId);

                int countMinions = deleteVillainMinions.ExecuteNonQuery();

                SqlCommand deleteVillain = new SqlCommand($@"DELETE FROM Villains WHERE Id = @villainId", connection);
                deleteVillain.Parameters.AddWithValue("@villainId", inputId);

                deleteVillain.ExecuteNonQuery();

                Console.WriteLine($"{villainName} was deleted.");
                Console.WriteLine($"{countMinions} minions were released.");
            }
        }
    }
}
