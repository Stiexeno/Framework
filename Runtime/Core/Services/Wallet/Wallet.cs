using System;
using Framework.Wallet;

namespace Framework.Core
{
	public class Wallet : IWallet
	{
		public Currency SoftCurrency { get; private set; }
		public Currency HardCurrency { get; private set; }
		
		public Wallet(IDataManager dataManager, WalletConfig config)
		{
			WalletData data;
			
			SoftCurrency = config.softCurrency;
			HardCurrency = config.hardCurrency;

			if (dataManager.HasData<WalletData>() == false)
			{
				data = dataManager.GetOrCreateData<WalletData>();

				if (SoftCurrency)
				{
					data.softCurrency.amount = SoftCurrency.initAmount;
				}
				
				if (HardCurrency)
				{
					data.hardCurrency.amount = HardCurrency.initAmount;
				}
			}
			else
			{
				data = dataManager.GetOrCreateData<WalletData>();
			}

			if (SoftCurrency)
			{
				SoftCurrency.Set(data.softCurrency);
			}
			
			if (HardCurrency)
			{
				HardCurrency.Set(data.hardCurrency);
			}
		}
	}   
}
