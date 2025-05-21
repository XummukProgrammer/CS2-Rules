# CS2-Rules
Данный плагин позволяет показать меню правил на сервере.

# Конфиг
Конфиг находится по пути `addons/counterstrikesharp/configs/plugins/CS2-Rules/CS2-Rules.json`.
> Файл генерируется автоматически при запуске сервера.

    // Тут необходимо распределить правила по категориям.
    // ==
    // ID - идентификатор правила.
    // В переводе:
    // [ID].title - как выглядит правило в меню
    // [ID].description - описание правила (в отдельном меню)
    // ==
    // Category - категория. При открытии меню сначала показываются категории, 
    // а уже через них можно переключится на правила.
    // В переводе:
    // [Category].title - как выглядит пункт категории в меню.
    // ==
    "Items": [
        {
          "ID": "Rule_1",
          "Category": "Category_1"
        },
        {
          "ID": "Rule_2",
          "Category": "Category_1"
        },
        {
          "ID": "Rule_3",
          "Category": "Category_2"
        }
      ],
      // Как отображать пункты категорий в меню (Их порядок).
      // Если окажется так, что в Items указана категория, которой нет здесь,
      // то пункт в категории будет создан после CategoriesOrder.
      "CategoriesOrder": [
        "Category_1",
        "Category_2"
      ],
      // Тип меню.
      // - ChatMenu
      // - ConsoleMenu
      // - CenterHtmlMenu
      // - WasdMenu
      // - ScreenMenu
      "MenusType": "ScreenMenu",
      // Сколько секунд меню будет открыто прежде чем автоматически закроется.
      "MenusDisplayTime": 10,
      // Замораживать ли игрока при открытии меню.
      "MenusIsFreeze": true,
      // Показывать ли меню при подключении игрока.
      "ShowRulesMenuOnPlayerConnected": true,
      // Через сколько секунд показывать меню при подключении игрока.
      "ShowRulesMenuDelay": 3

# Установка
1. Скачать [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp/releases/tag/v1.0.318)
2. Скачать [Metamod](https://www.sourcemm.net/downloads.php?branch=dev)
3. Скачать [релиз](https://github.com/XummukProgrammer/CS2-Rules/releases/tag/v1.0)
4. Распаковать архив из релиза на сервер
5. Настроить конфиг
6. Настроить переводы
7. Запускаем сервер и проверяем
