using CtorInjectionCrud.Models;
using CtorInjectionCrud.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly SqlConnection _dbConnection;

    public EmployeeRepository(SqlConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public IEnumerable<Employee> GetAllEmployees()
    {
        var employees = new List<Employee>();
        using (var command = new SqlCommand("SELECT * FROM Employees", _dbConnection))
        {
            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    employees.Add(new Employee
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = Convert.ToString(reader["Name"]),
                        ContactNumber = Convert.ToString(reader["ContactNumber"])
                    });
                }
            }
            _dbConnection.Close();
        }
        return employees;
    }

    public Employee GetEmployeeById(int id)
    {
        Employee employee = null;
        using (var command = new SqlCommand("SELECT * FROM Employees WHERE Id = @Id", _dbConnection))
        {
            command.Parameters.AddWithValue("@Id", id);
            _dbConnection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    employee = new Employee
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = Convert.ToString(reader["Name"]),
                        ContactNumber = Convert.ToString(reader["ContactNumber"])
                    };
                }
            }
            _dbConnection.Close();
        }
        return employee;
    }

    public void AddEmployee(Employee employee)
    {
        using (var command = new SqlCommand("INSERT INTO Employees (Name, ContactNumber) VALUES (@Name, @ContactNumber)", _dbConnection))
        {
            command.Parameters.AddWithValue("@Name", employee.Name);
            command.Parameters.AddWithValue("@ContactNumber", employee.ContactNumber);

            _dbConnection.Open();
            command.ExecuteNonQuery();
            _dbConnection.Close();
        }
    }

    public void UpdateEmployee(Employee employee)
    {
        using (var command = new SqlCommand("UPDATE Employees SET Name = @Name, ContactNumber = @ContactNumber WHERE Id = @Id", _dbConnection))
        {
            command.Parameters.AddWithValue("@Name", employee.Name);
            command.Parameters.AddWithValue("@ContactNumber", employee.ContactNumber);
            command.Parameters.AddWithValue("@Id", employee.Id);

            _dbConnection.Open();
            command.ExecuteNonQuery();
            _dbConnection.Close();
        }
    }

    public void DeleteEmployee(int id)
    {
        using (var command = new SqlCommand("DELETE FROM Employees WHERE Id = @Id", _dbConnection))
        {
            command.Parameters.AddWithValue("@Id", id);

            _dbConnection.Open();
            command.ExecuteNonQuery();
            _dbConnection.Close();
        }
    }
}
