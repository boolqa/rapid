Компоновка плагинов:
A. Сборка по шаблону проекта Razor Class Library (новый .NET) - Все в одном. Когда не планируешь шарить плагин или он только ui.
B. Сборка Class Library (новый .NET) - Только логика без UI.
C. Две сборки Class Library + Razor Class Library раздельно (новый .NET) - Логика и UI раздельно. 
Когда планируешь шарить другим плагинам одну (или обе) части плагина.

При создании нового проекта плагина:
1. Плагин создается в физической папке src/Plugins

2. Не забудь сменить путь выходной папки сборки в файле .csproj

2.2 Обязательно добавляем этот параметр, чтобы пути ниже заработали
<AppendTargetFrameworkToOutputPath>false</AppendTargetFrameworkToOutputPath>

2.3.1 Если у вас плагин в виде одной сборки
<OutputPath>..\..\Boolqa.Rapid.App\bin\$(Configuration)\$(TargetFramework)\plugins\$(MSBuildProjectName)</OutputPath>

2.3.2 Для отдельной сборки Ui плагина
<OutputPath>..\..\Boolqa.Rapid.App\bin\$(Configuration)\$(TargetFramework)\plugins\$(MSBuildProjectName.Replace(".Ui", ""))</OutputPath>

2.3.3 Ручной вариант
<OutputPath>..\..\Boolqa.Rapid.App\bin\$(Configuration)\$(TargetFramework)\plugins\{СамостоятельноУкажитеИмяПлагинаДляОбоихСборок}</OutputPath>

p.s Можно так же оставить бид в свою bin папку и написать action after build.