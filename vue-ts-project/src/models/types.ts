
export class List1 {
    UserId = 0;
    ListId = 0;
    Title = "";
    listItem = Array<ListItem1>();
    id = 0;
};

export class ListItem1 {
    Completed = false;
    ListItemText = "";
    id = 0;
    created = "";
    dueDate = "";
    priority = "";
    category = "";
}

export class User {
    Id = 0;
    FirstName = "";
    LastName = "";
    Token = "";
    UserName = "";
}

export class UserSettings {
    id = 0;
    UserId = 0;
    Category = "";
    ColorScheme = 0
    CategoryId = 0;
}