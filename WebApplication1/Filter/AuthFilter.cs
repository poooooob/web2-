using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class AuthFilter : IFilterMetadata
{
	//每个action执行之前都会执行这个方法
	public void OnActionExecuting(ActionExecutingContext context)
	{
		//判断session中是否有用户名
		if(context.HttpContext.Session.GetString("UserName") == null)
		{
			//判断是否有AllowAnonymous特性
			if(!HasAllowAnonymous(context))
			{
				//没有登录，跳转到登录页面
				context.Result = new RedirectResult("/Home/ShowLogin");
			}
		}
	}


	//判断是否有AllowAnonymous特性
	private bool HasAllowAnonymous(ActionExecutingContext context)
	{
		var endpoint = context.HttpContext.GetEndpoint();
		if(endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
		{
			return true;
		}
		return false;
	}

}