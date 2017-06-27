using Xamarin.Forms;
using Newtonsoft.Json;

namespace MyClassLibrary
{
    public class MemoryService
    {

        public User DeserializeUser()
        {
            var u = Application.Current.Properties["user"];
            User user = JsonConvert.DeserializeObject<User>(u.ToString());
            return user;
        }

        public void SerializeUser(User user)
        {
            Application.Current.Properties["user"] = JsonConvert.SerializeObject(user);
        }

        public void SaveString(string key ,string value)
        {
            if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
            {
                Application.Current.Properties[key] = value;
            }
        }

        public string GetString(string key)
        {
            return Application.Current.Properties[key].ToString();
        }

    }
}
