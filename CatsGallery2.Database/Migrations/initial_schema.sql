-- Создание таблиц Identity
CREATE TABLE IF NOT EXISTS aspnetusers (
                                           id SERIAL PRIMARY KEY,
                                           username VARCHAR(256) NOT NULL,
    normalized_username VARCHAR(256) NOT NULL,
    email VARCHAR(256),
    normalized_email VARCHAR(256),
    email_confirmed BOOLEAN NOT NULL DEFAULT FALSE,
    password_hash TEXT,
    security_stamp TEXT,
    concurrency_stamp TEXT,
    phone_number TEXT,
    phone_number_confirmed BOOLEAN NOT NULL DEFAULT FALSE,
    two_factor_enabled BOOLEAN NOT NULL DEFAULT FALSE,
    lockout_end TIMESTAMP WITH TIME ZONE,
                              lockout_enabled BOOLEAN NOT NULL DEFAULT FALSE,
                              access_failed_count INTEGER NOT NULL DEFAULT 0,
                              created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                              );

CREATE TABLE IF NOT EXISTS aspnetroles (
                                           id SERIAL PRIMARY KEY,
                                           name VARCHAR(256) NOT NULL,
    normalized_name VARCHAR(256) NOT NULL,
    concurrency_stamp TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
    );

CREATE TABLE IF NOT EXISTS aspnetuserroles (
                                               user_id INTEGER NOT NULL,
                                               role_id INTEGER NOT NULL,
                                               PRIMARY KEY (user_id, role_id),
    CONSTRAINT fk_aspnetuserroles_users FOREIGN KEY (user_id)
    REFERENCES aspnetusers(id) ON DELETE CASCADE,
    CONSTRAINT fk_aspnetuserroles_roles FOREIGN KEY (role_id)
    REFERENCES aspnetroles(id) ON DELETE CASCADE
    );

CREATE TABLE IF NOT EXISTS aspnetuserclaims (
                                                id SERIAL PRIMARY KEY,
                                                user_id INTEGER NOT NULL,
                                                claim_type TEXT,
                                                claim_value TEXT,
                                                CONSTRAINT fk_aspnetuserclaims_users FOREIGN KEY (user_id)
    REFERENCES aspnetusers(id) ON DELETE CASCADE
    );

CREATE TABLE IF NOT EXISTS aspnetuserlogins (
                                                login_provider TEXT NOT NULL,
                                                provider_key TEXT NOT NULL,
                                                provider_display_name TEXT,
                                                user_id INTEGER NOT NULL,
                                                PRIMARY KEY (login_provider, provider_key),
    CONSTRAINT fk_aspnetuserlogins_users FOREIGN KEY (user_id)
    REFERENCES aspnetusers(id) ON DELETE CASCADE
    );

CREATE TABLE IF NOT EXISTS aspnetusertokens (
                                                user_id INTEGER NOT NULL,
                                                login_provider TEXT NOT NULL,
                                                name TEXT NOT NULL,
                                                value TEXT,
                                                PRIMARY KEY (user_id, login_provider, name),
    CONSTRAINT fk_aspnetusertokens_users FOREIGN KEY (user_id)
    REFERENCES aspnetusers(id) ON DELETE CASCADE
    );

-- Бизнес-таблицы
CREATE TABLE IF NOT EXISTS catproducts (
                                           id SERIAL PRIMARY KEY,
                                           name VARCHAR(255) NOT NULL,
    description TEXT,
    price DECIMAL(10,2),
    application_user_id INTEGER NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_catproducts_users FOREIGN KEY (application_user_id)
    REFERENCES aspnetusers(id) ON DELETE CASCADE
    );

CREATE TABLE IF NOT EXISTS catimages (
                                         id SERIAL PRIMARY KEY,
                                         foreign_id VARCHAR(255) NOT NULL,
    url VARCHAR(500) NOT NULL,
    cat_product_id INTEGER NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_catimages_catproducts FOREIGN KEY (cat_product_id)
    REFERENCES catproducts(id) ON DELETE CASCADE
    );

CREATE TABLE IF NOT EXISTS tags (
                                    id SERIAL PRIMARY KEY,
                                    name VARCHAR(100) NOT NULL UNIQUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
    );

CREATE TABLE IF NOT EXISTS tagproducts (
                                           id SERIAL PRIMARY KEY,
                                           tag_id INTEGER NOT NULL,
                                           cat_product_id INTEGER NOT NULL,
                                           created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                                           CONSTRAINT fk_tagproducts_tags FOREIGN KEY (tag_id) REFERENCES tags(id),
    CONSTRAINT fk_tagproducts_catproducts FOREIGN KEY (cat_product_id)
    REFERENCES catproducts(id) ON DELETE CASCADE,
    CONSTRAINT uq_tagproduct UNIQUE (tag_id, cat_product_id)
    );

CREATE TABLE IF NOT EXISTS wishlistcats (
                                            id SERIAL PRIMARY KEY,
                                            application_user_id INTEGER NOT NULL,
                                            cat_product_id INTEGER NOT NULL,
                                            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                                            CONSTRAINT fk_wishlistcats_users FOREIGN KEY (application_user_id)
    REFERENCES aspnetusers(id) ON DELETE CASCADE,
    CONSTRAINT fk_wishlistcats_catproducts FOREIGN KEY (cat_product_id)
    REFERENCES catproducts(id) ON DELETE CASCADE,
    CONSTRAINT uq_userwishlist UNIQUE (application_user_id, cat_product_id)
    );