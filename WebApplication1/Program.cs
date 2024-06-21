var builder = WebApplication.CreateBuilder(args);//�������ú͹���Ӧ�ó���



//����������

//����session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();


// Add services to the container.
builder.Services.AddControllersWithViews(option =>
{
	//�����һ����֤������ AuthFilter���ù���������ÿ������������ͼִ��֮ǰ������֤��顣
	option.Filters.Add(new AuthFilter());
});
//����Razor �������Session������ע��
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //ע���� IHttpContextAccessor �ӿڵ�ʵ�֣��Ա������ Razor ��ͼ���ܹ����� HTTP �����ģ����а��� Session ����Ϣ��

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

//·��ע��
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=ShowLogin}/{id?}");


app.UseSession(); //����session

app.Run();
