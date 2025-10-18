# ===== Stage 1: Build =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# نسخ ملفات الحل والمشاريع
COPY *.sln ./
COPY Demo.PL/*.csproj Demo.PL/
COPY Demo.BLL/*.csproj Demo.BLL/
COPY Demo.DAL/*.csproj Demo.DAL/

# استعادة الحزم
RUN dotnet restore Demo.PL/Demo.PL.csproj

# نسخ باقي الملفات
COPY . .

# نشر المشروع في وضع Release إلى مجلد /app/out
RUN dotnet publish Demo.PL/Demo.PL.csproj -c Release -o /app/out

# ===== Stage 2: Runtime =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# نسخ الملفات المنشورة من مرحلة البناء
COPY --from=build /app/out .

# تأكد أن التطبيق يستمع لأي منفذ توفره Render
ENV ASPNETCORE_URLS=http://+:$PORT

# (اختياري) يمكنك تحديد البيئة Production
ENV ASPNETCORE_ENVIRONMENT=Production

# فتح المنفذ المحلي (Render سيستخدم $PORT تلقائيًا)
EXPOSE 5000

# تشغيل التطبيق
ENTRYPOINT ["dotnet", "Demo.PL.dll"]
