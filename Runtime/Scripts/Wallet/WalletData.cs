using System;
using Framework.Core;

namespace Framework.Wallet
{
	[Serializable]
	public class WalletData : IData
	{
		public CurrencyRef softCurrency = new CurrencyRef();
		public CurrencyRef hardCurrency = new CurrencyRef();
	}

	[Serializable]
	public class CurrencyRef
	{
		public BigNumber amount;
	}
}