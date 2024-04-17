function initTransactionForm(url) {
    $("#TransactionTypeId").change(async function () {
        const $selectedValue = $(this).val();

        const res = await fetch(url, {
            method: "POST",
            body: $selectedValue,
            headers: {
                "Content-Type": "application/json"
            }
        });

        const json = await res.json();

        const $options = json.map(el => `<option value=${el.value}>${el.text}</option>`);
        $("#CategoryId").html($options);
    })
}