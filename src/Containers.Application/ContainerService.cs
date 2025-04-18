using Containers.Models;
using Microsoft.Data.SqlClient;

namespace Containers.Application;

public class ContainerService : IContainerService
{
    private string _connectionString;
    
    public ContainerService(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public IEnumerable<Container> GetAllContainers()
    {
        List<Container> containers = [];
        const string query = "SELECT * FROM Containers";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var containerRow = new Container
                        {
                            Id = reader.GetInt32(0),
                            ContainerTypeId = reader.GetInt32(1),
                            IsHazardious = reader.GetBoolean(2),
                            Name = reader.GetString(3)
                        };
                        
                        containers.Add(containerRow);
                    }
                }
            }
            finally
            {
                reader.Close();
            }
            return containers;
        }
    }

    public bool AddContainer(Container container)
    {
        const string insertString =
            "INSERT INTO Containers (ContainerTypeId, IsHazardious, Name) VALUES (@ContainerTypeId, @IsHazardious, @Name)";

        int countRowsAdded = -1;
        
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(insertString, connection);
            command.Parameters.AddWithValue("@ContainerTypeId", container.ContainerTypeId);
            command.Parameters.AddWithValue("@IsHazardious", container.IsHazardious);
            command.Parameters.AddWithValue("@Name", container.Name);
            
            connection.Open();
            countRowsAdded = command.ExecuteNonQuery();
        }
        
        return countRowsAdded != -1;
    }
}