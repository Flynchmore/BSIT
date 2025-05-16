-- Table structure for table customer
CREATE TABLE customer (
    CustomerID INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(255) NULL,
    Email VARCHAR(255) NULL,
    Password VARCHAR(255) NULL,
    Address TEXT NULL,
    `Contact Number` VARCHAR(255) NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Table structure for table customer_order
CREATE TABLE customer_order (
  OrderID INT AUTO_INCREMENT PRIMARY KEY,
  CustomerID INT NOT NULL,
  DispatcherID INT NOT NULL,
  FoodID INT NOT NULL,
  Quantity INT NOT NULL,
  OrderDate DATETIME DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (CustomerID) REFERENCES customer(CustomerID) ON DELETE CASCADE,
  FOREIGN KEY (DispatcherID) REFERENCES dispatcher(DispatcherID) ON DELETE CASCADE,
  FOREIGN KEY (FoodID) REFERENCES food_menu(FoodID) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Table structure for table dispatcher
CREATE TABLE dispatcher (
    DispatcherID INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(255) NULL,
    Email VARCHAR(255) NULL,
    Password VARCHAR(255) NULL,
    Address TEXT NULL,
    `Contact Number` VARCHAR(255) NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Table structure for table food_menu
CREATE TABLE food_menu (
  FoodID INT AUTO_INCREMENT PRIMARY KEY,
  Name VARCHAR(255) DEFAULT NULL,
  Price DECIMAL(10,2) DEFAULT NULL,
  Category INT DEFAULT NULL,
  Stacks INT DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Table structure for table manager
CREATE TABLE manager (
    ManagerID INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(255) NULL,
    Email VARCHAR(255) NULL,
    Password VARCHAR(255) NULL,
    Address TEXT NULL,
    `Contact Number` VARCHAR(255) NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;