using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Timers;
using CS2MenuManager.API.Class;
using CS2MenuManager.API.Enum;
using CS2MenuManager.API.Menu;
using System.Text.Json.Serialization;

namespace CS2_Rules;

public class MenuItemModel
{
    [JsonPropertyName("ID")]
    public string ID { get; set; } = string.Empty;

    [JsonPropertyName("Category")]
    public string CategoryID { get; set; } = string.Empty;
}

public class ConfigModel : BasePluginConfig
{
    [JsonPropertyName("Items")]
    public List<MenuItemModel> Items { get; set; } = 
    [
        new() { ID = "Rule_1", CategoryID = "Category_1" },
        new() { ID = "Rule_2", CategoryID = "Category_1" },
        new() { ID = "Rule_3", CategoryID = "Category_2" },
    ];

    [JsonPropertyName("CategoriesOrder")]
    public List<string> CategoriesOrder { get; set; } = 
    [
        "Category_1",
        "Category_2",    
    ];

    [JsonPropertyName("MenusType")]
    public string MenusType { get; set; } = typeof(ScreenMenu).Name;

    [JsonPropertyName("MenusDisplayTime")]
    public int MenusDisplayTime { get; set; } = 10;

    [JsonPropertyName("MenusIsFreeze")]
    public bool MenusIsFreeze { get; set; } = true;

    [JsonPropertyName("ShowRulesMenuOnPlayerConnected")]
    public bool ShowRulesMenuOnPlayerConnected { get; set; } = true;

    [JsonPropertyName("ShowRulesMenuDelay")]
    public float ShowRulesMenuDelay { get; set; } = 3;
}

public class CS2Rules : BasePlugin, IPluginConfig<ConfigModel>
{
    public override string ModuleName => "Rules";
    public override string ModuleVersion => "v1.0";
    public override string ModuleAuthor => "Xummuk97";

    public ConfigModel Config { get; set; } = null!;

    private readonly Dictionary<string, List<MenuItemModel>> _categoriesData = [];

    public void OnConfigParsed(ConfigModel config)
    {
        Config = config;

        _categoriesData.Clear();

        foreach (var categoryID in Config.CategoriesOrder)
            _categoriesData.Add(categoryID, []);

        foreach (var item in Config.Items)
            if (_categoriesData.TryGetValue(item.CategoryID, out var items))
                items.Add(item);
            else
                _categoriesData.Add(item.CategoryID, [item]);
    }

    [ConsoleCommand("css_rules", "Opens the rules menu.")]
    public void OnRulesCommand(CCSPlayerController? controller, CommandInfo _)
    {
        ShowCategoriesMenu(controller);
    }

    [GameEventHandler]
    public HookResult OnPlayerConnectFullPost(EventPlayerConnectFull @event, GameEventInfo _)
    {
        if (Config.ShowRulesMenuOnPlayerConnected)
        {
            var controller = @event.Userid;
            if (controller == null || !controller.IsValid || controller.IsBot || controller.IsHLTV)
                return HookResult.Continue;

            AddTimer(Config.ShowRulesMenuDelay, () => ShowCategoriesMenu(controller), TimerFlags.STOP_ON_MAPCHANGE);
        }

        return HookResult.Continue;
    }

    private void ShowCategoriesMenu(CCSPlayerController? controller)
    {
        if (controller == null) 
            return;

        var menu = CreateMenu(Localizer["menus.categories.title"]);

        foreach (var categoryData in _categoriesData)
            AddCategoryInMenu(controller, menu, categoryData.Key, categoryData.Value);

        menu.Display(controller, Config.MenusDisplayTime);
    }

    private void AddCategoryInMenu(CCSPlayerController controller, BaseMenu menu, string categoryID, List<MenuItemModel> items)
    {
        int count = items.Count;
        bool hasItems = count > 0;
        DisableOption disableOption = hasItems ? DisableOption.None : DisableOption.DisableShowNumber;

        string category = Localizer[$"{categoryID}.title"];
        string name = Localizer["menus.categories.item", category, count];

        menu.AddItem(name, (_, _) => ShowRulesMenu(controller, menu, categoryID, items), disableOption);
    }

    private void ShowRulesMenu(CCSPlayerController? controller, BaseMenu prevMenu, string categoryID, List<MenuItemModel> items)
    {
        if (controller == null) 
            return;

        var menu = CreateMenu(Localizer["menus.rules.title"], prevMenu);

        foreach (var item in items)
            AddRuleInMenu(controller, menu, item.ID);

        menu.Display(controller, Config.MenusDisplayTime);
    }

    private void AddRuleInMenu(CCSPlayerController controller, BaseMenu menu, string ruleID)
    {
        string name = Localizer[$"{ruleID}.title"];

        menu.AddItem(name, (_, _) => ShowRuleMenu(controller, menu, ruleID), DisableOption.None);
    }

    private void ShowRuleMenu(CCSPlayerController? controller, BaseMenu prevMenu, string ruleID)
    {
        if (controller == null) 
            return;

        var menu = CreateMenu(Localizer["menus.rule.title"], prevMenu);

        AddRuleInfoInMenu(controller, menu, ruleID);

        menu.Display(controller, Config.MenusDisplayTime);
    }

    private void AddRuleInfoInMenu(CCSPlayerController controller, BaseMenu menu, string ruleID)
    {
        var description = Localizer[$"{ruleID}.description"];

        menu.AddItem(description, DisableOption.DisableHideNumber);
    }

    private BaseMenu CreateMenu(string title, BaseMenu? prevMenu = null)
    {
        var menu = MenuManager.MenuByType(Config.MenusType, title, this);
        menu.PrevMenu = prevMenu;
        menu.ScreenMenu_FreezePlayer = Config.MenusIsFreeze;
        menu.WasdMenu_FreezePlayer = Config.MenusIsFreeze;

        return menu;
    }
}
