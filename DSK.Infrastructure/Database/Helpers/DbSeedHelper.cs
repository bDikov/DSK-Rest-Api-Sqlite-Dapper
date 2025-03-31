using DSK.Infrastructure.Database.Enums;
using DSK.Infrastructure.Database.Models;
using Microsoft.Data.Sqlite;
using System.Data.SQLite;

namespace DSK.Infrastructure.Database.Helpers
{
    public static class DbSeedHelper
    {
        public static void CreateDatabase(string connectionString)
        {
            var directoryPath = Path.GetDirectoryName(connectionString);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath!);


                // Check if the database file already exists  
                if (!File.Exists(connectionString))
                {
                    try
                    {
                        // Create the database file  
                        SQLiteConnection.CreateFile(connectionString);
                        Console.WriteLine("Database created successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error occurred while creating database: {ex.Message}");
                    }


                    // Create the connection  
                    using var connection = new SqliteConnection($"Data Source={connectionString};");
                    connection.Open();

                    // Start a transaction  
                    using var transaction = connection.BeginTransaction();
                    try
                    {
                        // Command to create Credits table  
                        string createCreditTableQuery = @"
                            CREATE TABLE IF NOT EXISTS Credits (  
                            Id TEXT  PRIMARY KEY NOT NULL,
                            Number TEXT NOT NULL,  
                            Name TEXT NOT NULL,  
                            Amount DECIMAL  NOT NULL,  
                            RequestDate TEXT NOT NULL,  
                            Status INTEGER NOT NULL     
                        );  ";

                        // Command to create Invoices table  
                        string createInvoiceTableQuery = @"  
                              CREATE TABLE IF NOT EXISTS Invoices (  
                              Id TEXT  PRIMARY KEY NOT NULL,
                              CreditId TEXT NOT NULL,          
                              CreatedOn TEXT NOT NULL,         
                              Number TEXT NOT NULL,  
                              Amount DECIMAL  NOT NULL,  
                              FOREIGN KEY (CreditId) REFERENCES Credits(Id) ON DELETE CASCADE  
                        );";

                        // Execute Credit table creation with transaction  
                        using var creditCommand = new SqliteCommand(createCreditTableQuery, connection, transaction);
                        creditCommand.ExecuteNonQuery();

                        // Execute Invoice table creation with transaction  
                        using var invoiceCommand = new SqliteCommand(createInvoiceTableQuery, connection, transaction);
                        invoiceCommand.ExecuteNonQuery();

                        // Commit transaction  
                        transaction.Commit();
                        Console.WriteLine("Tables created successfully.");

                        SeedDatabase(connection);
                    }
                    catch (Exception ex)
                    {
                        // Roll back the transaction in case of errors  
                        transaction.Rollback();
                        Console.WriteLine($"Error occurred during table creation: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Database already exists.");
                }
            }
        }
        private static void SeedDatabase(SqliteConnection connection)
        {
            // Creating GUIDs for the credits  
            var credit1Id = Guid.NewGuid().ToString();
            var credit2Id = Guid.NewGuid().ToString();
            var credit3Id = Guid.NewGuid().ToString();

            // Seed data for Credits  
            var credits = new List<CreditDbModel>
            {
                new() { Id = credit1Id, Number = "CREDIT001", Name = "Alice Smith", Amount = 5000, RequestDate = DateTime.UtcNow, Status = CreditStatusType.Created },
                new() { Id = credit2Id, Number = "CREDIT002", Name = "Bob Johnson", Amount = 15000, RequestDate = DateTime.UtcNow, Status = CreditStatusType.AwaitingPayment },
                new() { Id = credit3Id, Number = "CREDIT003", Name = "Charlie Brown", Amount = 200000, RequestDate = DateTime.UtcNow, Status = CreditStatusType.Paid }
            };

            // Insert Credits into the database  
            foreach (var credit in credits)
            {
                using var insertCommand = new SqliteCommand("INSERT INTO Credits (Id, Number, Name, Amount, RequestDate, Status) VALUES (@id, @number, @name, @amount, @requestDate, @status)", connection);
                insertCommand.Parameters.AddWithValue("@id", credit.Id); // Convert GUID to byte array  
                insertCommand.Parameters.AddWithValue("@number", credit.Number);
                insertCommand.Parameters.AddWithValue("@name", credit.Name);
                insertCommand.Parameters.AddWithValue("@amount", credit.Amount);
                insertCommand.Parameters.AddWithValue("@requestDate", credit.RequestDate.ToString("o")); // ISO 8601 format  
                insertCommand.Parameters.AddWithValue("@status", (int)credit.Status);
                insertCommand.ExecuteNonQuery();
            }

            // Now we will also insert some Invoices associated with the credits  
            var invoices = new List<InvoiceDbModel>
            {
                new() { Id = Guid.NewGuid().ToString(), CreditId = credit1Id, CreatedOn = DateTime.UtcNow, Number = "INV001", Amount = 100 },
                new() { Id = Guid.NewGuid().ToString(), CreditId = credit1Id, CreatedOn = DateTime.UtcNow, Number = "INV002", Amount = 200 },
                new() { Id = Guid.NewGuid().ToString(), CreditId = credit2Id, CreatedOn = DateTime.UtcNow, Number = "INV003", Amount = 1320 },
                new() { Id = Guid.NewGuid().ToString(), CreditId = credit2Id, CreatedOn = DateTime.UtcNow, Number = "INV004", Amount = 330 },
                new() { Id = Guid.NewGuid().ToString(), CreditId = credit3Id, CreatedOn = DateTime.UtcNow, Number = "INV005", Amount = 5010 }
            };

            // Inserting Invoices into the database  
            foreach (var invoice in invoices)
            {
                using var insertCommand = new SqliteCommand("INSERT INTO Invoices (Id, CreditId, CreatedOn, Number, Amount) VALUES (@id, @creditId, @createdOn, @number, @amount)", connection);
                insertCommand.Parameters.AddWithValue("@id", invoice.Id);
                insertCommand.Parameters.AddWithValue("@creditId", invoice.CreditId);
                insertCommand.Parameters.AddWithValue("@createdOn", invoice.CreatedOn.ToString("o"));
                insertCommand.Parameters.AddWithValue("@number", invoice.Number);
                insertCommand.Parameters.AddWithValue("@amount", invoice.Amount);
                insertCommand.ExecuteNonQuery();
            }
        }
    }

}