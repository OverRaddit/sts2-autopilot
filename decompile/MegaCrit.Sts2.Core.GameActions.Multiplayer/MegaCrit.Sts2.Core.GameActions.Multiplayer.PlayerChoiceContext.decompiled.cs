using System.Collections.Generic;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;

namespace MegaCrit.Sts2.Core.GameActions.Multiplayer;

public abstract class PlayerChoiceContext
{
	private Stack<AbstractModel>? _modelStack;

	public AbstractModel? LastInvolvedModel
	{
		get
		{
			Stack<AbstractModel>? modelStack = _modelStack;
			AbstractModel result = default(AbstractModel);
			if (modelStack == null || !modelStack.TryPeek(ref result))
			{
				return null;
			}
			return result;
		}
	}

	public void PushModel(AbstractModel model)
	{
		if (_modelStack == null)
		{
			_modelStack = new Stack<AbstractModel>();
		}
		_modelStack.Push(model);
	}

	public void PopModel(AbstractModel model)
	{
		AbstractModel abstractModel = default(AbstractModel);
		if (_modelStack == null || !_modelStack.TryPeek(ref abstractModel) || abstractModel != model)
		{
			Log.Error($"Tried to pop model {model} from from stack of player choice context {this} but it wasn't on the top of the stack!");
		}
		else
		{
			_modelStack.Pop();
		}
	}

	public abstract System.Threading.Tasks.Task SignalPlayerChoiceBegun(PlayerChoiceOptions options);

	public abstract System.Threading.Tasks.Task SignalPlayerChoiceEnded();
}
