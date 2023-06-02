using System.Collections;
using Framework.Core;
using UnityEngine;

namespace Framework.Utils
{
	public class AutoSaver : MonoBehaviour
	{
		//Private fields
		
		private IDataManager dataManager;
		
		private WaitForSeconds autoSaveWaitForSeconds = new WaitForSeconds(30f);

		//Properties

		[Inject]
		private void Construct(IDataManager dataManager)
		{
			this.dataManager = dataManager;
			
			StartCoroutine(AutoSaveCoroutine());
		}

		private IEnumerator AutoSaveCoroutine()
		{
			while (true)
			{
				yield return autoSaveWaitForSeconds;
				dataManager.SaveAll();
			}
		}
	}
}