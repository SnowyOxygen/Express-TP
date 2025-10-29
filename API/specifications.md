## Product

### Create product:
As a store owner, I want to create a new product with details
such as name, description, price, and stock quantity so that I can add it to my store's inventory.

- Given a product with an existing name, I should receive an error indicating that the product already exists.
- Given a product with a price below 0 or above 30 000, I should receive an error indicating that the price is invalid.
- Given a product with a sale price equal or above the price, I should receive an error indicating that the sale price is invalid.
- Given a product with a stock quantity below 0, I should receive an error indicating that the stock quantity is invalid.
- Given a product with a title or a description below two characters, I should receive an error indicating that the title or description is too short.
- Given a product with a title above 100 or a description above 500 characters, I should receive an error indicating that the title or description is too long.
- Given a product with a non existing category, I should receive an error indicating that the category is invalid.

- Given a product with valid details, the product should be created successfully and added to the inventory.

### Get all products:
As a store owner, I want to retrieve a list of all products in my store's inventory

- Given no products exist in the inventory, I should receive an empty list.
- Given multiple products exist in the inventory, I should receive a list containing all products with their details.

----
## Category

### Create category:
As a store owner, I want to create a new category with a name and description so that I can organize my products.

- Given a category with an existing name, I should receive an error indicating that the category already exists.
- Given a category with a name below two characters, I should receive an error indicating that the name is too short.
- Given a category with a name above 100 characters, I should receive an error indicating that the name is too long.
- Given a category with a description above 500 characters, I should receive an error indicating that the description is too long.
- Given a category with an invalid color, I should receive an error indicating that the color is invalid. 

- Given a category with valid details, the category should be created successfully.

### Get all categories:
As a store owner, I want to retrieve a list of all categories in my store's inventory

- Given no categories exist in the inventory, I should receive an empty list.
- Given multiple categories exist in the inventory, I should receive a list containing all categories with their details.

----
## Order

### Create Order:
As a customer, I want to place an order for existing products. The order should contain a list of products,
the quantity bought and at which price they were bought.

- Given an order with a product that does not exist, I should receive an error indicating that the product is invalid.
- Given an order with a quantity below 1, I should receive an error indicating that the quantity is invalid.
- Given an order with a quantity above the available stock, I should receive an error indicating insufficient stock.
- Given an order with an empty product list, I should receive an error indicating that the order cannot be empty.
- Given an order with more than five products, I should recieve an error.
- Given an order with correct details, the order should be created successfully, and the stock quantity of the products should be updated accordingly.

### Update Order lines:
#### Inferred from the rule: "si commande existe, incrementer lignes"
As a customer, I want to be able to update an existing order to remove or modify the quantity
of products ordered.

- Given an order with five products and a product is added, I should recieve an error indicating there are too many.
- Given an order line with a quantity change that would result in below 0 or above 5, I should recieve an error.
- Given an order line with a quantity change with insufficient stock, I should recieve an error.
- Given an order line with quantity 0, remove it from the order.
- Given an order with no more order lines, the order should be cancelled
- Given an order with status "paid" or "cancelled", I should recieve an error
- Given an order with correct details, the order lines should be updated

### Pay order:
As a customer, I want to be able to set the status of an order to "paid"

- Given an order with status "paid", I should recieve an error indicating it is already paid.
- Given an order with status "cancelled", I should recieve an error
- Given an order with correct details, the status of the order should be set to "paid"

### Cancel order:
As a customer, I want to be able to set the status of an order to "cancelled"

- Given an order with status "cancelled", I should recieve an error indicating it is already cancelled.
- Given an order with correct details, the status of the order should be set to cancelled.