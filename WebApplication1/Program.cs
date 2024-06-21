var builder = WebApplication.CreateBuilder(args);//用于配置和构建应用程序。



//启动配置项

//开启session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();


// Add services to the container.
builder.Services.AddControllersWithViews(option =>
{
	//添加了一个认证过滤器 AuthFilter，该过滤器将在每个控制器和视图执行之前进行认证检查。
	option.Filters.Add(new AuthFilter());
});
//增加Razor 引擎访问Session的依赖注入
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //注册了 IHttpContextAccessor 接口的实现，以便后续在 Razor 视图中能够访问 HTTP 上下文，其中包括 Session 等信息。

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//路由注册
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=ShowLogin}/{id?}");


app.UseSession(); //启动session

app.Run();
