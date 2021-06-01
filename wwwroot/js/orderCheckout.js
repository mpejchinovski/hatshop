function createProductTableRow(item) {
    let row = document.createElement('tr');
    row.innerHTML = `
                <td class="w-25 mh-25"><img src="${item.image}" class="ml-5 rounded-circle order-hat-img"></td>
                <td>${item.name}</td>
                <td>$${item.price}</td>
                <td>${item.quantity}</td>
                <td>${item.description}</td>
                <td>$${item.quantity * item.price}</td>
                <td>
                    <button class="btn btn-danger btn-sm rounded-0 remove-from-cart" type="button" data-hat-id=${item.id}>
                        <i class="fa fa-trash"></i>
                    </button>
                </td>`;

    $(row).find('button').click(function () {
        $(this).parents('tr').eq(0).remove();
    });

    return row;
}

$(document).ready(function () {
    for (let item of orderItems)
        $('.cart-table-body').append(createProductTableRow(item));

    $('.cart-table-body').append(`
    <tr>
        <td><strong>Order total:</strong></td>
        <td></td>
        <td></td>
        <td></td>
        <td></td>
        <td class="order-total">$` + orderItems.reduce((acc, item) => acc + item.price * item.quantity, 0) + `</td>
	<td></td></tr>`
    );

    $('.create-order').click(function () {
        $.ajax({
            type: "POST",
            url: "/api/orders",
            contentType: "application/json",
            data: JSON.stringify(orderItems.map(item => ({ HatId: item.hatId, Quantity: item.quantity }))),
            success: data => {
                console.log(data);
                if (data.statusCode == 200) {
                    alert('Order successful!');
                    clearCart();
                    window.location.href = "/Orders";
                }
                else {
                    alert('Error creating order!');
                }
            },
        })
    });
});
