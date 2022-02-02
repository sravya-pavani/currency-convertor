# currency-convertor

```
Usage:  
ExchangeRateCalculator <FROM_CURRENCYCODE> <TO_CURRENCYCODE> <AMOUNT> [<DATE>]  
OR  
ExchangeRateCalculator backupToDb  
Supported Currencies: USD AUD CAD PLN MXN EUR  
```

---
**NOTE**
In order to use backupToDb option in proper way:
- The SQL settings have to be propely configured in app.config
- The code has an assumption about Table name as EXCHANGE_HISTORY, if needed this is to be changed
- The Table is presumed to be created already
---
