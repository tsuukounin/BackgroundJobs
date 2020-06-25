using System.Threading.Tasks;

namespace Tsuukounin.BackgroundJobs.Example.Sevices
{
	public interface ITimeConsumingTaskService
	{
		Task DoWork(string name = "Unnamed");
	}
}
