using System.Threading.Tasks;

namespace Tsuukounin.BackgroundJobs.Core
{
	public interface IBackgroundJobsService
	{
		void Execute(Task task);
		void ExecuteInNamedQueue(string queueName, Task task);
	}
}
