document.addEventListener("DOMContentLoaded", function () {
    var id = document.querySelector("table").id;
    const rows = document.querySelectorAll(`#${id} tbody tr`);
    const totalRecordsLabel = document.getElementById("totalRecords");
    const paginationContainer = document.getElementById("pagination");
    const rowsPerPageSelect = document.getElementById("rowsPerPage");

    let currentPage = 1;
    let rowsPerPage = parseInt(rowsPerPageSelect.value);

    function renderTable() {
        const start = (currentPage - 1) * rowsPerPage;
        const end = start + rowsPerPage;

        rows.forEach((row, index) => {
            row.style.display = (index >= start && index < end) ? "" : "none";
        });

        totalRecordsLabel.textContent = `Total Records: ${rows.length}`;
        renderPagination();
    }

    function renderPagination() {
        paginationContainer.innerHTML = "";
        const pageCount = Math.ceil(rows.length / rowsPerPage);

        // Prev button
        const prev = document.createElement("button");
        prev.textContent = "<";
        prev.disabled = currentPage === 1;
        prev.onclick = () => {
            if (currentPage > 1) {
                currentPage--;
                renderTable();
            }
        };
        paginationContainer.appendChild(prev);

        const maxVisible = 3;
        let startPage = currentPage - 1;
        let endPage = currentPage + 1;

        if (startPage < 1) {
            startPage = 1;
            endPage = Math.min(pageCount, maxVisible);
        }
        if (endPage > pageCount) {
            endPage = pageCount;
            startPage = Math.max(1, endPage - maxVisible + 1);
        }

        // Always show first page
        if (startPage > 1) {
            addPageButton(1);
            if (startPage > 2) addEllipsis();
        }

        // Show visible pages
        for (let i = startPage; i <= endPage; i++) {
            addPageButton(i);
        }

        // Always show last page
        if (endPage < pageCount) {
            if (endPage < pageCount - 1) addEllipsis();
            addPageButton(pageCount);
        }

        // Next button
        const next = document.createElement("button");
        next.textContent = ">";
        next.disabled = currentPage === pageCount;
        next.onclick = () => {
            if (currentPage < pageCount) {
                currentPage++;
                renderTable();
            }
        };
        paginationContainer.appendChild(next);
    }

    function addPageButton(page) {
        const btn = document.createElement("button");
        btn.textContent = page;
        btn.className = (page === currentPage) ? "active" : "";
        btn.onclick = () => {
            currentPage = page;
            renderTable();
        };
        paginationContainer.appendChild(btn);
    }

    function addEllipsis() {
        const span = document.createElement("span");
        span.textContent = "...";
        span.style.margin = "0 6px";
        paginationContainer.appendChild(span);
    }

    rowsPerPageSelect.addEventListener("change", function () {
        rowsPerPage = parseInt(this.value);
        currentPage = 1;
        renderTable();
    });

    renderTable();
});
    