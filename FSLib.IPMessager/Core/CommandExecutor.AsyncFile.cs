using System;
using System.Linq;
using System.Net;
using System.Threading;
using FSLib.IPMessager.Define;
using FSLib.IPMessager.Entity;
using FSLib.IPMessager.Network;
using FSLib.IPMessager.Properties;


namespace FSLib.IPMessager.Core
{
	/// <summary>
	/// 负责命令的解析等操作
	/// </summary>
	public partial class CommandExecutor : IDisposable
	{
		/// <summary>
		/// 发送同步消息
		/// </summary>
		public void SendHeartBeatMessage()
		{
			ulong options = 0ul;
			if (Config.IsInAbsenceMode) options |= (ulong)Consts.Cmd_All_Option.Absence;
			if (Config.EnableFileTransfer) options |= (ulong)Define.Consts.Cmd_All_Option.FileAttach;
			//插件
			if (Config.Services != null) Config.Services.ProviderExecute(s => options |= s.GenerateClientFeatures());


			//广播
			MessageProxy.SendWithNoCheck(null, Consts.Commands.SendMsg, options | (ulong)Consts.Cmd_Send_Option.BroadCast, new Guid().ToString(), Config.GroupName);

			//单个通知
			options |= (ulong)Consts.Cmd_All_Option.DialUp;

			//Config.KeepedHostList_Addr.ForEach(s => MessageProxy.SendByIp(new IPEndPoint(s, Config.Port), Consts.Commands.SendMsg, options, Config.NickName, Config.GroupName));
		}

	}

}
