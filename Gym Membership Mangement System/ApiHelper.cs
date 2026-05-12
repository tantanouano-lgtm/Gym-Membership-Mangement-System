using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class ApiHelper
{
    private static readonly HttpClient client = new HttpClient();
    private const string BaseUrl = "http://localhost/GMS/api/";  // FIXED: removed "functions/"

    // GET - Fetch all members
    public static async Task<string> GetMembers()
    {
        HttpResponseMessage response = await client.GetAsync(BaseUrl + "members.php");
        return await response.Content.ReadAsStringAsync();
    }

    // POST - Add new member
    public static async Task<string> AddMember(object memberData)
    {
        string json = JsonConvert.SerializeObject(memberData);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync(BaseUrl + "members.php", content);
        return await response.Content.ReadAsStringAsync();
    }

    // PUT - Update member
    public static async Task<string> UpdateMember(object memberData)
    {
        string json = JsonConvert.SerializeObject(memberData);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PutAsync(BaseUrl + "members.php", content);
        return await response.Content.ReadAsStringAsync();
    }

    // DELETE - Delete member
    public static async Task<string> DeleteMember(int id)
    {
        HttpResponseMessage response = await client.DeleteAsync(BaseUrl + "members.php?id=" + id);
        return await response.Content.ReadAsStringAsync();
    }

    // GET - Fetch all staff
    public static async Task<string> GetStaff()
    {
        HttpResponseMessage response = await client.GetAsync(BaseUrl + "staff.php");  // FIXED: use staff.php which actually exists
        return await response.Content.ReadAsStringAsync();
    }

    // POST - Add new staff
    public static async Task<string> AddStaff(object staffData)
    {
        string json = JsonConvert.SerializeObject(staffData);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync(BaseUrl + "staff.php", content);  // FIXED: use staff.php
        return await response.Content.ReadAsStringAsync();
    }

    // POST - Add new trainer
    public static async Task<string> AddTrainer(object trainerData)
    {
        string json = JsonConvert.SerializeObject(trainerData);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync(BaseUrl + "trainers.php", content);
        return await response.Content.ReadAsStringAsync();
    }

    // GET - Fetch all equipment
    public static async Task<string> GetEquipment()
    {
        HttpResponseMessage response = await client.GetAsync(BaseUrl + "equipment.php");  // FIXED: now uses BaseUrl
        return await response.Content.ReadAsStringAsync();
    }

    // POST - Admin/Member Login
    public static async Task<string> Login(object loginData)
    {
        string json = JsonConvert.SerializeObject(loginData);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync(BaseUrl + "login.php", content);
        return await response.Content.ReadAsStringAsync();
    }

    // POST - Register new member account
    public static async Task<string> RegisterMember(object registerData)
    {
        string json = JsonConvert.SerializeObject(registerData);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PostAsync(BaseUrl + "register_admin.php", content);
        return await response.Content.ReadAsStringAsync();
    }

    // DELETE - Delete equipment
    public static async Task<string> DeleteEquipment(int id)
    {
        HttpResponseMessage response = await client.DeleteAsync(BaseUrl + "equipment.php?id=" + id);  // FIXED: now uses BaseUrl
        return await response.Content.ReadAsStringAsync();
    }
    // PUT - Update staff
    public static async Task<string> UpdateStaff(int id, object staffData)
    {
        string json = JsonConvert.SerializeObject(staffData);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await client.PutAsync(BaseUrl + "staff.php?id=" + id, content);
        return await response.Content.ReadAsStringAsync();
    }

    // DELETE - Delete staff
    public static async Task<string> DeleteStaff(int id)
    {
        HttpResponseMessage response = await client.DeleteAsync(BaseUrl + "staff.php?id=" + id);
        return await response.Content.ReadAsStringAsync();
    }
}