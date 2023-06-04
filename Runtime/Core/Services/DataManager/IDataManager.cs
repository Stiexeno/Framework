namespace Framework.Core
{
	public interface IDataManager
	{
		void SaveData<T>(T data);
		void SaveAll();
		T LoadRawData<T>() where T : class, IData, new();
		T GetOrCreateData<T>() where T : class, IData, new();
		bool HasData<T>();
		void RegisterData<T>(T data);
		void DeleteData<T>() where T : class, IData, new();
	}
}