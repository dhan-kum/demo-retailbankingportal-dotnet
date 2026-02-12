// React bundle placeholder
// This is a minimal placeholder for the React frontend
// In a real implementation, this would be built from React source files

(function() {
    'use strict';
    
    const rootElement = document.getElementById('root');
    
    if (rootElement) {
        rootElement.innerHTML = `
            <div style="padding: 20px;">
                <h1>Eviden Retail Banking Portal</h1>
                <p>Welcome to the .NET 8.0 Banking Portal</p>
                <div style="margin-top: 20px;">
                    <h2>Bank Accounts</h2>
                    <div id="accounts-container">Loading accounts...</div>
                </div>
                <div style="margin-top: 20px;">
                    <h2>Transfer Funds</h2>
                    <div>
                        <input type="text" id="from-account" placeholder="From Account" />
                        <input type="text" id="to-account" placeholder="To Account" />
                        <input type="number" id="amount" placeholder="Amount" />
                        <button onclick="transferFunds()">Transfer</button>
                    </div>
                </div>
            </div>
        `;
        
        // Fetch and display accounts
        fetch('/api/bankaccounts')
            .then(response => response.json())
            .then(accounts => {
                const container = document.getElementById('accounts-container');
                container.innerHTML = accounts.map(account => `
                    <div class="bank-account">
                        <div class="account-number">Account: ${account.accountNumber}</div>
                        <div>Name: ${account.accountName}</div>
                        <div>Type: ${account.accountType}</div>
                        <div class="balance">Balance: $${account.balance.toFixed(2)}</div>
                    </div>
                `).join('');
            })
            .catch(error => {
                console.error('Error fetching accounts:', error);
                document.getElementById('accounts-container').innerHTML = 
                    '<p>Error loading accounts</p>';
            });
    }
    
    // Make transferFunds function global
    window.transferFunds = function() {
        const fromAccount = document.getElementById('from-account').value;
        const toAccount = document.getElementById('to-account').value;
        const amount = parseFloat(document.getElementById('amount').value);
        
        if (!fromAccount || !toAccount || !amount) {
            alert('Please fill in all fields');
            return;
        }
        
        fetch('/api/bankaccounts/transfer', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                fromAccount: fromAccount,
                toAccount: toAccount,
                amount: amount
            })
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Transfer failed');
            }
            return response.json();
        })
        .then(data => {
            alert(data.message);
            window.location.reload();
        })
        .catch(error => {
            console.error('Error:', error);
            alert('Transfer failed: ' + error.message);
        });
    };
})();
