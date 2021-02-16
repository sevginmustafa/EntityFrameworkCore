using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace _05.ChangeTownNamesCasing
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection connection = new SqlConnection(@"Server=.\SQLEXPRESS; Database=MinionsDB; Integrated Security=true");
            connection.Open();

            string country = Console.ReadLine();

            using (connection)
            {
                SqlCommand checkIfCountryExists = new SqlCommand(@"SELECT Id FROM Countries WHERE Name = @country", connection);
                checkIfCountryExists.Parameters.AddWithValue("@country", country);

                int? countryId = (int?)checkIfCountryExists.ExecuteScalar();

                if (countryId == null)
                {
                    Console.WriteLine("No town names were affected.");
                    return;
                }

                SqlCommand checkIfCountryHaveTowns = new SqlCommand($@"SELECT Name FROM Towns WHERE CountryCode = {countryId}", connection);

                string countries = (string)checkIfCountryHaveTowns.ExecuteScalar();

                if (countries == null)
                {
                    Console.WriteLine("No town names were affected.");
                    return;
                }

                SqlCommand updateToUpper = new SqlCommand($@"UPDATE Towns SET Name = UPPER(Name)
                                                             WHERE CountryCode = (SELECT Id FROM Countries WHERE Name = '{country}')", connection);

                int countriesAffected = updateToUpper.ExecuteNonQuery();

                Console.WriteLine($"{countriesAffected} town names were affected.");

                SqlDataReader reader = checkIfCountryHaveTowns.ExecuteReader();

                List<string> countryNames = new List<string>();

                using (reader)
                {
                    while (reader.Read())
                    {
                        countryNames.Add((string)reader["Name"]);
                    }
                }

                Console.WriteLine($"[{string.Join(", ", countryNames)}]");
            }
        }
    }
}
