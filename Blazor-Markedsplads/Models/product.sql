CREATE TABLE product (
  id INTEGER PRIMARY KEY,
  product_type TEXT,
  product_name TEXT,
  percent NUMERIC,
  nationality TEXT,
  age INTEGER,
  price NUMERIC,
  
  customer_id INTEGER,
  FOREIGN KEY (customer_id) REFERENCES               customer(id),
  FOREIGN KEY (alcohol_id) REFERENCES                alcohol(id),
  FOREIGN KEY (product_type) REFERENCES
  alcohol (alcotype)
);