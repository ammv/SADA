using DataLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace Test
{
    internal class Program
    {
        public static IQueryable<T> Cast<T>(object o)
        {
            return (IQueryable<T>)o;
        }

        private static Type GetDbSetType<T>(DbSet<T> data) where T : class
        {
            return typeof(T);
        }

        private static void Main(string[] args)
        {
            DbSet<User> dummy;

            using (var ctx = new SADAEntities())
            {
                var property = ctx.GetType().GetProperty("User");

                Type type = property.PropertyType;

                dynamic dbset = property.GetValue(ctx);

                MethodInfo mi = typeof(Enumerable).GetMethod("ToList").MakeGenericMethod(GetDbSetType(dbset));

                dynamic list = mi.Invoke(null, new object[] { dbset });

                List<User> dummy2 = new List<User>();

                //dummy.

                Console.WriteLine(list.Any());
            }

            //dummy.ToList

            Console.Read();
        }
        //private static void TestViewModelTT()
        //{
        //    string nspace = "Test";
        //    List<Type> types = new List<Type>();

        //    var q = from t in Assembly.GetExecutingAssembly().GetTypes()
        //            where t.IsClass && t.Namespace.StartsWith(nspace) && !t.Name.Contains('<')
        //            select t;
        //    q.ToList().ForEach(t => types.Add(t));

        //    List<Type> viewTypes = types.Where(t => t.Name.EndsWith("View")).ToList();
        //    List<Type> viewModelTypes = types.Where(t => t.Name.EndsWith("ViewModel")).ToList();

        //    Dictionary<string, List<string>> viewFolders = new Dictionary<string, List<string>>();
        //    Dictionary<string, List<string>> viewModelFolders = new Dictionary<string, List<string>>();

        //    List<string> viewFolderFull = new List<string>();
        //    List<string> viewModelFolderFull = new List<string>();

        //    foreach (var viewType in viewTypes)
        //    {
        //        var parts = viewType.FullName.Split('.');

        //        string folderName = string.Join("", parts.Skip(1).Take(parts.Length - 2));

        //        if (!viewFolders.ContainsKey(folderName))
        //        {
        //            viewFolders.Add(folderName, new List<string>());
        //        }

        //        viewFolders[folderName].Add(viewType.Name);

        //        viewFolderFull.Add(string.Join(".", parts.Take(parts.Length - 1)));
        //    }

        //    foreach (var viewModelType in viewModelTypes)
        //    {
        //        var parts = viewModelType.FullName.Split('.');

        //        string folderName = string.Join("", parts.Skip(1).Take(parts.Length - 2));

        //        if (!viewModelFolders.ContainsKey(folderName))
        //        {
        //            viewModelFolders.Add(folderName, new List<string>());
        //        }

        //        viewModelFolders[folderName].Add(viewModelType.Name);

        //        viewModelFolderFull.Add(string.Join(".", parts.Take(parts.Length - 1)));
        //    }

        //    var viewKeys = viewFolders.Keys.ToList();

        //    for (int i = 0; i < viewFolders.Count; i++)
        //    {
        //        var m = viewFolders[viewKeys[i]]; //viewModelFolderFull[i]#>
        //    }

        //    Dictionary<string, string> viewViewModels = new Dictionary<string, string>();

        //    foreach (var viewFolder in viewFolders)
        //    {
        //        var viewModelFolder = $"{viewFolder.Key.Replace("View", "ViewModel")}";
        //        if (!viewModelFolders.ContainsKey(viewModelFolder))
        //        {
        //            continue;
        //        }

        //        var views = viewFolder.Value;

        //        foreach (var viewModel in viewModelFolders[viewModelFolder])
        //        {
        //            var targetView = views.FirstOrDefault(v => viewModel.StartsWith(v));
        //            if (targetView != null)
        //            {
        //                viewViewModels.Add($"{viewFolder.Key}:{targetView}", $"{viewModelFolder}:{viewModel}");
        //            }
        //        }
        //    }

        //    foreach (var vvm in viewViewModels)
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append($"<DataTemplate DataType = \"{{x:Type {vvm.Value}}}\">\n");
        //        sb.Append($"\t<{vvm.Key}/>\n");
        //        sb.Append($"</DataTemplate>\n\n");
        //    }
        //}
    }
}