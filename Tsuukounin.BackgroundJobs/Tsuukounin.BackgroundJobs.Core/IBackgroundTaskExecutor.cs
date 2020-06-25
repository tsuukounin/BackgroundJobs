using System.Threading.Tasks;

namespace Tsuukounin.BackgroundJobs.Core
{
	public interface IBackgroundTaskExecutor
	{
		void AddTask(Task task);
	}
}
