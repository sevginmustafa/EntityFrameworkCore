using System;
using Microsoft.Data.SqlClient;

namespace _04.AddMinion
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] minionData = Console.ReadLine().Split();
            string minionName = minionData[1];
            string minionAge = minionData[2];
            string minionTown = minionData[3];

            string[] villainData = Console.ReadLine().Split();
            string villainName = villainData[1];

            SqlConnection connection = new SqlConnection(@"Server=.\SQLEXPRESS; Database = MinionsDB; Integrated Security=true");
            connection.Open();

            using (connection)
            {
                SqlCommand commandCheckIfTownExists = new SqlCommand(@"SELECT Id FROM Towns WHERE Name = @minionTown", connection);
                commandCheckIfTownExists.Parameters.AddWithValue("@minionTown", minionTown);

                int? townId = (int?)commandCheckIfTownExists.ExecuteScalar();

                if (townId == null)
                {
                    SqlCommand commandInsertNewTown = new SqlCommand(@"INSERT INTO Towns(Name, CountryCode)VALUES
                                                                        (@minionTown, NULL)", connection);
                    commandInsertNewTown.Parameters.AddWithValue("@minionTown", minionTown);

                    commandInsertNewTown.ExecuteNonQuery();

                    Console.WriteLine($"Town {minionTown} was added to the database.");

                    townId = (int)commandCheckIfTownExists.ExecuteScalar();
                }


                SqlCommand commandCheckIfMinionExists = new SqlCommand(@"SELECT Id FROM Minions WHERE Name = @minionName", connection);
                commandCheckIfMinionExists.Parameters.AddWithValue("@minionName", minionName);

                int? minionId = (int?)commandCheckIfMinionExists.ExecuteScalar();

                if (minionId == null)
                {
                    SqlCommand commandInsertNewMinion = new SqlCommand(@"INSERT INTO Minions(Name, Age, TownId) VALUES
                                                                         (@minionName, @minionAge, @townId)", connection);
                    commandInsertNewMinion.Parameters.AddWithValue("@minionName", minionName);
                    commandInsertNewMinion.Parameters.AddWithValue("@minionAge", minionAge);
                    commandInsertNewMinion.Parameters.AddWithValue("@townId", townId);

                    commandInsertNewMinion.ExecuteNonQuery();

                    minionId = (int)commandCheckIfMinionExists.ExecuteScalar();
                }


                SqlCommand commandCheckVillain = new SqlCommand(@"SELECT Id FROM Villains WHERE Name = @villainName", connection);
                commandCheckVillain.Parameters.AddWithValue("@villainName", villainName);

                int? villainId = (int?)commandCheckVillain.ExecuteScalar();

                if (villainId == null)
                {
                    SqlCommand commandInsertNewVillain = new SqlCommand(@"INSERT INTO Villains(Name, EvilnessFactorId) VALUES
                                                                          (@villainName, 4)", connection);
                    commandInsertNewVillain.Parameters.AddWithValue("@villainName", villainName);

                    commandInsertNewVillain.ExecuteNonQuery();

                    Console.WriteLine($"Villain {villainName} was added to the database.");

                    villainId = (int)commandCheckVillain.ExecuteScalar();
                }


                SqlCommand commandCheckIfVillainHasAnyMinions = new SqlCommand(@"SELECT MinionId FROM MinionsVillains WHERE VillainId = @villainId", connection);
                commandCheckIfVillainHasAnyMinions.Parameters.AddWithValue("@villainId", villainId);

                int? minionsOfVillain = (int?)commandCheckIfVillainHasAnyMinions.ExecuteScalar();

                if (minionsOfVillain == null)
                {
                    SqlCommand commandAddVillainAndSetMinionToIt = new SqlCommand($@"INSERT INTO MinionsVillains(MinionId, VillainId) VALUES
                                                                             (@minionId, @villainId)", connection);
                    commandAddVillainAndSetMinionToIt.Parameters.AddWithValue("@minionId", minionId);
                    commandAddVillainAndSetMinionToIt.Parameters.AddWithValue("@villainId", villainId);

                    commandAddVillainAndSetMinionToIt.ExecuteNonQuery();

                    Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
                }
                else
                {
                    SqlCommand commandCheckIfMinionIsAlreadySetToVillain = new SqlCommand(@"SELECT VillainId FROM MinionsVillains 
                                                                                            WHERE MinionId = @minionId AND VillainId = @villainId", connection);
                    commandCheckIfMinionIsAlreadySetToVillain.Parameters.AddWithValue("@minionId", minionId);
                    commandCheckIfMinionIsAlreadySetToVillain.Parameters.AddWithValue("@villainId", villainId);

                    int? checkedMinionVillainId = (int?)commandCheckIfMinionIsAlreadySetToVillain.ExecuteScalar();

                    if (checkedMinionVillainId != villainId)
                    {
                        SqlCommand commandSetMinionToVillain = new SqlCommand($@"INSERT INTO MinionsVillains(MinionId, VillainId) VALUES
                                                                             (@minionId, @villainId)", connection);
                        commandSetMinionToVillain.Parameters.AddWithValue("@minionId", minionId);
                        commandSetMinionToVillain.Parameters.AddWithValue("@villainId", villainId);

                        commandSetMinionToVillain.ExecuteNonQuery();

                        Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
                    }
                }
            }
        }
    }
}
