function createCartEntry({ id, name, description, image, price, quantity } = {}) {
    return `<li id=${id} class="cart-entry">
                <div class="card mb-3" style="max-width: 540px; max-height: 230px; overflow: clip">
                    <div class="row g-0">
                        <div class="col-md-4" style="height: 100px; position: relative; top: 20px; left: 12px;">
                            <img src="${image}" alt="..." class="img-fluid rounded-circle cart-hat-image" />
                        </div>
                        <div class="col-md-8">
                            <div class="card-body" style="margin-left: 12px">
                                <h6 class="card-title cart-hat-title">${quantity} x ${name}</h6>
                                <p class="card-text cart-hat-description">
                                </p>
                                <span class="text-muted">Total: </span>
                                <h6 class="card-text cart-hat-total">$${price * quantity}</h6>
                                <button class="btn btn-danger btn-thinner remove-from-cart" data-hat-id=${id}>Remove</button>
                            </div>
                        </div>
                    </div>
                </div>
            </li>`;
}
