using Framework.Core;
using UnityEngine;
using SF = UnityEngine.SerializeField;

namespace Framework.Wallet
{
	public class WalletConfig : AbstractConfig
	{
		public Currency softCurrency;
		public Currency hardCurrency;
	}   
}
