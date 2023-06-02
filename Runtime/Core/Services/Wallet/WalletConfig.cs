using Framework.Core;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Wallet
{
	[CreateAssetMenu(fileName = "WalletConfig", menuName = "Framework/Wallet/WalletConfig")]
	public class WalletConfig : ConfigBase
	{
		public Currency softCurrency;
		public Currency hardCurrency;
	}   
}
