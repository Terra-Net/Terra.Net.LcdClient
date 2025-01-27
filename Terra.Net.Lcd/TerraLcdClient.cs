﻿
using Microsoft.Extensions.Logging;
using Terra.Net.Lcd.Helpers;
using Terra.Net.Lcd.Interfaces;
using Terra.Net.Lcd.Objects;

namespace Terra.Net.Lcd
{
    public class TerraLcdClient : BaseRestClient, ITerraLcdClient
    {
        #region Endpoints
        private const string GetBlockByHeightOldUrl = "/blocks/{}";
        private const string GetGasPricesUrl = "/v1/txs/gas_prices";
        private const string GetMempoolUrl = "/v1/mempool?account=";
        private const string GetTxInMempoolUrl = "/v1/mempool/{}";
        private const string GetTxUrl = "/v1/tx/{}";
        private const string GetTransactionsListUrl = "/v1/tx";
        
        private const string GetBlockByHeightUrl = "/cosmos/base/tendermint/v1beta1/blocks/{}";
        private const string SimulateUrl = "/cosmos/tx/v1beta1/simulate";





        #endregion
        public TerraLcdClient(TerraClientOptions exchangeOptions, ILogger<TerraLcdClient> logger) : base(exchangeOptions, logger)
        {
        }
        #region Blocks
        public async Task<CallResult<BlockResponseOld>> GetBlockByHeightOldAsync(ulong height, CancellationToken ct = default)
        {
            return await Get<BlockResponseOld>(GetBlockByHeightUrl.FillPathParameters(height.ToString()), null, ct);
        }

        public async Task<CallResult<BlockResponseOld>> GetLatestBlockOldAsync(CancellationToken ct = default)
        {
            return await Get<BlockResponseOld>(GetBlockByHeightUrl.FillPathParameters("latest"), null, ct);
        }
        #endregion
        #region Legacy tx endpoints
        public async Task<CallResult<GasPrices>> GetGasPrices(CancellationToken ct = default)
        {
            var data = await Get<Dictionary<string, ulong>>(GetGasPricesUrl, ct: ct);
            if (data)
            {
                return new CallResult<GasPrices>(new GasPrices(data.Result));

            }
            else
            {
                return new CallResult<GasPrices>(default(GasPrices), data.Error);
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="address"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<CallResult<MempoolResponse>> GetMempool(string? address = null, CancellationToken ct = default)
        {
            return await Get<MempoolResponse>(GetMempoolUrl + address, ct: ct);
        }
        #endregion

    }
}